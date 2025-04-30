import { useEffect } from "react";
import { userManager } from "@/auth/userManager";

export default function CallbackPage() {
  useEffect(() => {
    userManager.signinRedirectCallback()
      .then(user => {
        console.log("User logged in", user);
        window.location.href = "/";
      })
      .catch(err => {
        console.error("Error in callback", err);
      });
  }, []);

  return (
    <div>Processing login...</div>
  );
}