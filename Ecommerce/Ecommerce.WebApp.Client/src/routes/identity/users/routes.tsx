import { useRoutes } from "react-router-dom";
import { ListUser } from "./list";
import { CreateUser } from "./create";
import { CloneUser } from "./clone";
import { ShowUser } from "./show";

export const UserRoutes = () => {
  const routes = useRoutes([
    {
      path: "/",
      element: <ListUser />,
    },
    {
      path: "create",
      element: <CreateUser />,
    },
    {
      path: ":id/clone",
      element: <CloneUser />,
    },
    {
      path: ":id",
      element: <ShowUser />,
    },
  ]);

  return routes;
};
