# ManagerLicense — chạy với Docker

Workspace gồm 7 project ASP.NET Core 9 và một ứng dụng React 19/Vite. Không có tham chiếu project chéo; mỗi project .NET có solution riêng.

| Thư mục | Vai trò | Địa chỉ Docker trên máy local |
| --- | --- | --- |
| `my-admin-dashboard` | Dashboard React, đăng nhập OIDC, giao diện quản trị | http://localhost:3000 |
| `ApiGateway` | YARP chuyển `/api` sang API và các endpoint OIDC sang AuthServer | http://localhost:7130 |
| `AuthServer` | ASP.NET Identity + OpenIddict, Authorization Code/PKCE | http://localhost:7130/account/login |
| `EcommerceApi` | CRUD sản phẩm, danh mục, menu; Entity Framework + SQL Server | http://localhost:7130/api/products |
| `Ecommerce` | Website MVC và client OIDC | http://localhost:7294 |
| `LicenseManagerApp` | Tạo license, ký RSA và xác minh | http://localhost:5237/License/Create |
| `MvcApp` | Client gọi LicenseManagerApp và kiểm tra license định kỳ | http://localhost:5080 |
| `ProductTest` | Dịch vụ demo trả danh sách sản phẩm | http://localhost:5001/products |

Luồng chính: trình duyệt → dashboard → ApiGateway → EcommerceApi/AuthServer → SQL Server. Website Ecommerce dùng cùng AuthServer. MvcApp gọi LicenseManagerApp độc lập.

## Khởi động

Yêu cầu Docker Desktop đang chạy Linux containers. Chạy tại thư mục gốc:

```powershell
Copy-Item .env.example .env
# Sửa SQL_PASSWORD và ADMIN_PASSWORD trong .env trước khi chạy.
docker --context desktop-linux compose up -d --build
docker --context desktop-linux compose ps
node scripts/smoke-docker.mjs
```

Nếu `.env` đã được tạo trong lần triển khai đầu tiên, giữ nguyên file; không ghi đè mật khẩu của SQL Server đang có dữ liệu. Tài khoản đăng nhập là `admin`, mật khẩu lấy từ `ADMIN_PASSWORD` trong `.env`. Seeder chỉ tạo tài khoản khi chưa tồn tại; đổi `.env` không tự đổi mật khẩu tài khoản đã tạo.

Script smoke yêu cầu Node.js 20+, kiểm tra các trang/API, discovery/JWKS/CORS, đăng nhập admin với PKCE, refresh token và callback OIDC của website. Script tạo một license demo cho user `hung` và kiểm tra chữ ký RSA; không in mật khẩu hoặc token.

```powershell
docker --context desktop-linux compose logs --tail 100 auth-server ecommerce-api
docker --context desktop-linux compose stop
docker --context desktop-linux compose up -d
docker --context desktop-linux compose down
```

`down` giữ volume. Không dùng `down -v` nếu cần giữ dữ liệu. Các cổng công khai chỉ bind `127.0.0.1`; SQL Server, AuthServer và EcommerceApi chỉ truy cập trong mạng Compose. Không cần SQL Server hoặc chứng chỉ HTTPS trên Windows.

## Cấu hình và dữ liệu

- `compose.yaml`: 9 dịch vụ, địa chỉ nội bộ Docker, dependency và healthcheck.
- `Dockerfile`: build nhiều giai đoạn dùng chung cho 7 ứng dụng .NET.
- Dashboard dùng pnpm lockfile, build TypeScript/Vite rồi phục vụ bằng Nginx; đường dẫn SPA như `/callback` được trả về `index.html`.
- SQL Server Developer 2022 giữ dữ liệu trong `sql-data`. Ba ứng dụng dùng database riêng `AuthDatabase`, `EcommerceApi`, `EcommerceStore` vì hai project Ecommerce có schema sản phẩm khác nhau.
- `Database:Initialize=true` khởi tạo database trống từ EF model bằng `EnsureCreatedAsync`. Bộ migration hiện thiếu file designer/metadata; đây là bootstrap cho môi trường local, không phải quy trình nâng cấp schema. Khi model thay đổi cần chuẩn hóa migration trước khi nâng cấp database có dữ liệu.
- Volume `license-keys` giữ khóa RSA do ứng dụng tự sinh; khóa PEM có sẵn trong repo không đưa vào image. Volume `mvc-data` giữ license tải về của client.
- `.env` ở thư mục gốc chứa mật khẩu local và được bỏ qua bởi Git/Docker build. Các URL dashboard là cấu hình lúc build; đổi hostname/cổng cần cập nhật cả build, issuer và callback client.
- OIDC dùng issuer công khai `http://localhost:7130/`; website remap HTTP backchannel sang `http://api-gateway:8080/` để trao đổi token và đọc khóa trong mạng Docker.

## Giới hạn hiện có

Đây là triển khai phát triển trên máy local bằng HTTP và SQL Server Developer. Khi triển khai public cần HTTPS, cấu hình domain/callback, khóa ký OpenIddict bền vững và cơ chế migration phù hợp. OpenIddict hiện dùng khóa ephemeral, nên phiên/token có thể cần đăng nhập lại khi AuthServer khởi động lại.

LicenseManagerApp giữ danh sách license trong bộ nhớ; danh sách mất khi restart dù khóa RSA được giữ. Tạo license cho user `hung` trước khi MvcApp có thể tải license theo cấu hình mặc định; chu kỳ kiểm tra là 2 phút.

Dashboard có một số chức năng prototype: tạo/sửa sản phẩm hiện mới ghi console, xóa mới hiện alert. API hiện chưa áp dụng xác thực bearer cho các endpoint CRUD. Website Ecommerce còn dùng view và dữ liệu mẫu. Script trong `backup database` là snapshot cũ của OpenIddict, không dùng để bootstrap hệ thống mới.
