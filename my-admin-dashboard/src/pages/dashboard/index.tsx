import { ComposeEditor } from "@/components/compose/index";
import { useAuth } from "@/auth/auth-context";

export default function Dashboard() { 
  const { user } = useAuth();
  const draftKey = `article-draft:${user?.profile.sub ?? "local"}`;
  return (
    <div className="mx-auto max-w-5xl space-y-5">
      <div>
        <h1 className="text-2xl font-semibold text-gray-900">Viết bài</h1>
        <p className="mt-1 text-gray-500">Soạn nội dung và giữ bản nháp cho bài viết của bạn.</p>
      </div>
      <ComposeEditor key={draftKey} draftKey={draftKey} />
    </div>
  );
}
