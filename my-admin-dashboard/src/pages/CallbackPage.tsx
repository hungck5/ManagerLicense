import { useEffect } from "react";
import { useAuth } from "@/auth/auth-context";
import { useNavigate } from "react-router-dom";

export default function CallbackPage() {
  const { signinRedirectCallback } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    signinRedirectCallback().then(() => {
      navigate("/");
    });
  }, [signinRedirectCallback, navigate]);

  return (
    <div>Processing login...</div>
  );
}