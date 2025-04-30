import { userManager } from "../auth/userManager";

export default function Login() {
  return (
    
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="absolute top-20 left-1/2 transform -translate-x-1/2 bg-white p-8 rounded-lg shadow-lg border border-gray-300 w-96 min-h-96">
        <h1 className="text-4xl font-bold text-center text-gray-800 mb-8">Welcome to Admin page</h1>
        <div className="space-y-4">
          <button className="w-full py-2 px-4 bg-blue-500 text-white rounded-lg shadow-md hover:bg-blue-600 transition"
            onClick={() => userManager.signinRedirect()}>
            Đăng nhập
          </button>
          <button className="w-full py-2 px-4 bg-green-500 text-white rounded-lg shadow-md hover:bg-green-600 transition"
            onClick={() => userManager.signoutRedirect()}>
            Đăng ký tài khoản
          </button>
        </div>
      </div>
    </div>
  );
}