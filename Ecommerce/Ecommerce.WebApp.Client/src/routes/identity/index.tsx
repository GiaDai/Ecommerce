import { Outlet, useRoutes } from "react-router-dom";

export const IdentityRoutes = () => {
  const routes = useRoutes([
    {
      path: "/identity",
      element: (
        <>
          <h1>Identity</h1>
          <Outlet />
        </>
      ),
      children: [
        { path: "users", element: <h1>Users</h1> },
        { path: "roles", element: <h1>Roles</h1> },
        { path: "claims", element: <h1>Claims</h1> },
      ],
    },
  ]);

  return routes;
};
