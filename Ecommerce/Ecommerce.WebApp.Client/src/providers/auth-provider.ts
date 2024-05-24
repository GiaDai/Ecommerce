import { AuthProvider as BaseAuthProvider, HttpError } from "@refinedev/core";
import { jwtDecode } from "jwt-decode";
import { ResponseRoot } from "./types";
import { JwtTokenDecoded } from "@routes/authens";
interface ResponseAuthen {
  Succeeded: boolean;
  Message: string;
  Errors: any;
  Data: any;
}

interface RefreshTokenResponse {
  AccessToken: string;
  RefreshToken: string;
}

interface AuthProvider extends BaseAuthProvider {
  refresh: () => Promise<{ success: boolean }>;
}

export const authProvider: AuthProvider = {
  check: async () => {
    // When logging in, we'll obtain an access token from our API and store it in the local storage.
    // Now let's check if the token exists in the local storage.
    // In the later steps, we'll be implementing the `login` and `logout` methods.
    const token = localStorage.getItem("access_token");
    if (Boolean(token)) {
      return { authenticated: true };
    }
    return {
      authenticated: false,
      error: {
        message: "Check failed",
        name: "Not authenticated",
      },
      logout: true,
      redirectTo: "/login",
    };
  },
  getIdentity: async () => {
    const response = await fetch("/api/account/me", {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("access_token") ?? ""}`,
      },
    });

    if (response.status < 200 || response.status > 299) {
      localStorage.removeItem("access_token");
      // We're returning success: true to indicate that the logout operation was successful.
    }

    const data = (await response.json()) as ResponseRoot;
    if (!data.Succeeded) {
      const errorResponse = (await response.json()) as ResponseRoot;
      const error: HttpError = {
        message: errorResponse.Message,
        statusCode: errorResponse.Code,
      };
      return Promise.reject(error);
    }
    return data.Data as any;
  },
  login: async ({ email, password }) => {
    const response = await fetch("/api/account/authenticate", {
      method: "POST",
      body: JSON.stringify({ Email: email, Password: password }),
      headers: {
        "Content-Type": "application/json",
      },
    });

    const data = (await response.json()) as ResponseAuthen;
    console.log(data);
    if (data.Succeeded) {
      if (data.Data.JWToken) {
        localStorage.setItem("access_token", data.Data.JWToken);
        // localStorage.setItem("refresh_token", data.Data.RefreshToken);
        return {
          success: true,
          successNotification: {
            message: "Login Successful",
            description: "You have been successfully logged in.",
          },
          redirectTo: "/dashboard",
        };
      }
    }

    return {
      success: false,
      error: {
        name: "Login Failed!",
        message: data.Message ?? "Invalid email or password",
      },
    };
  },
  logout: async () => {
    localStorage.removeItem("access_token");
    localStorage.removeItem("refresh_token");

    // We're returning success: true to indicate that the logout operation was successful.
    return { success: true };
  },
  onError: async (error) => {
    return { error };
  },
  getPermissions: async () => {
    const token = localStorage.getItem("access_token");
    if (!token) {
      return [];
    }

    const decoded: JwtTokenDecoded = jwtDecode(token);
    return decoded.roles ?? "";
  },
  updatePassword: async ({ oldPassword, newPassword }) => {
    const response = await fetch("/api/account/update-password", {
      method: "POST",
      body: JSON.stringify({ oldPassword, newPassword }),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("access_token") ?? ""}`,
      },
    });

    const data = (await response.json()) as ResponseRoot;
    if (data.Succeeded) {
      return {
        success: true,
        successNotification: {
          message: "Password Updated",
          description: "Your password has been successfully updated.",
        },
        redirectTo: "/dashboard",
      };
    }
    return {
      success: false,
      error: {
        name: "Login Failed!",
        message: data.Message ?? "Invalid email or password",
      },
    };
  },
  refresh: async () => {
    const refreshToken = localStorage.getItem("refresh_token");
    const accessToken = localStorage.getItem("access_token");
    const response = await fetch("/api/account/refresh-token", {
      method: "POST",
      body: JSON.stringify({
        AccessToken: accessToken,
        RefreshToken: refreshToken,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    });
    if (response.ok) {
      const data = (await response.json()) as RefreshTokenResponse;
      localStorage.setItem("access_token", data.AccessToken);
      localStorage.setItem("refresh_token", data.RefreshToken);
      return { success: true };
    } else {
      localStorage.removeItem("access_token");
      localStorage.removeItem("refresh_token");
      return { success: false };
    }
  },
};
