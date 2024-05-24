import { AccessControlProvider } from "@refinedev/core";
import { authProvider } from "./auth-provider";
import { Roles } from "./types";

export const accessControlProvider: AccessControlProvider = {
  can: async ({ resource, action }) => {
    if (!authProvider || typeof authProvider.getPermissions !== "function") {
      return {
        can: false,
        reason: "AuthProvider or getPermissions is undefined",
      };
    }
    const roles = JSON.parse(
      (await authProvider.getPermissions()) as string
    ) as Roles;

    const { permissions } = roles;

    // if (role === "SuperAdmin") return { can: true };

    for (const permission of permissions) {
      if (
        permission.resource === resource &&
        permission.action.includes(action)
      ) {
        return { can: true };
      }
    }

    return {
      can: false,
      reason: "Unauthorized",
    };
  },
  options: {
    buttons: {
      enableAccessControl: true,
      hideIfUnauthorized: true,
    },
  },
};
