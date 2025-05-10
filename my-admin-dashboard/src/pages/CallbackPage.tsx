import { useEffect, useRef } from "react";
import { useAuth } from "@/auth/auth-context";
import { useNavigate } from "react-router-dom";

export default function CallbackPage() {
  const { signinRedirectCallback } = useAuth();
  const navigate = useNavigate();
  const calledRef = useRef(false);

  useEffect(() => {
    if (calledRef.current) return;
    calledRef.current = true;

    signinRedirectCallback().then(() => {
      navigate("/");
    });
  }, [signinRedirectCallback, navigate]);

  return (
    <div>Processing login...</div>
  );
}