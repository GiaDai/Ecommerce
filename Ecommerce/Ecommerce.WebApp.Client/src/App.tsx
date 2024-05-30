import { App as AntdApp, ConfigProvider } from "antd";
import React from "react";
import {
  AuthPage,
  ErrorComponent,
  ImageField,
  ThemedLayoutV2,
  ThemedTitleV2,
  useNotificationProvider,
} from "@refinedev/antd";
import routerProvider, {
  CatchAllNavigate,
  NavigateToResource,
} from "@refinedev/react-router-v6";
import { BrowserRouter, Outlet, Route, Routes } from "react-router-dom";
import { resources, themeConfig } from "./config";
import { Authenticated, CanAccess, Refine } from "@refinedev/core";
import { accessControlProvider, authProvider, dataProvider } from "./providers";
import {
  CloneUser,
  CreateUser,
  EditUser,
  ListUser,
  ShowUser,
} from "@routes/identity/users";
import {
  CloneProduct,
  CloneProductAttribute,
  CloneRole,
  CloneRoleClaim,
  CreateProduct,
  CreateProductAttribute,
  CreateRangeProduct,
  CreateRole,
  CreateRoleClaim,
  Dashboard,
  EditProduct,
  EditProductAttribute,
  EditRole,
  EditRoleClaim,
  ListProduct,
  ListProductAttribute,
  ListRole,
  ListRoleClaim,
  ShowProduct,
  ShowProductAttribute,
  ShowRole,
  ShowRoleClaim,
} from "./routes";
import { Unauthorized } from "@components/unauthorized";

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <ConfigProvider theme={themeConfig}>
        <AntdApp>
          <Refine
            dataProvider={dataProvider}
            authProvider={authProvider}
            routerProvider={routerProvider}
            accessControlProvider={accessControlProvider}
            notificationProvider={useNotificationProvider}
            options={{
              syncWithLocation: true,
              warnWhenUnsavedChanges: true,
            }}
            resources={resources}
          >
            <Routes>
              <Route
                element={
                  <Authenticated
                    key="authenticated-routes"
                    fallback={<CatchAllNavigate to="/login" />}
                  >
                    <ThemedLayoutV2
                      Title={({ collapsed }: any) => (
                        <ThemedTitleV2
                          collapsed={collapsed}
                          icon={
                            <ImageField
                              value="https://static.vietbank.com.vn/web/vietbank-logo.png"
                              title="Vietbank Logo"
                              style={{ width: 30, height: 30 }}
                            />
                          }
                          text="Vietbank Admin"
                        />
                      )}
                    >
                      <Outlet />
                    </ThemedLayoutV2>
                  </Authenticated>
                }
              >
                <Route
                  index
                  element={<NavigateToResource resource="dashboard" />}
                />
                <Route path="dashboard">
                  <Route
                    index
                    element={
                      <CanAccess
                        resource="dashboard"
                        action="list"
                        fallback={<Unauthorized />}
                      >
                        <Dashboard />
                      </CanAccess>
                    }
                  />
                </Route>
                <Route path="products">
                  <Route
                    index
                    element={
                      <CanAccess
                        resource="products"
                        action="list"
                        fallback={<Unauthorized />}
                      >
                        <ListProduct />
                      </CanAccess>
                    }
                  />
                  <Route
                    path="create"
                    element={
                      <CanAccess
                        resource="products"
                        action="create"
                        fallback={<Unauthorized />}
                      >
                        <CreateProduct />
                      </CanAccess>
                    }
                  />
                  <Route
                    path="create-range"
                    element={
                      <CanAccess
                        resource="products"
                        action="create-range"
                        fallback={<Unauthorized />}
                      >
                        <CreateRangeProduct />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/clone"
                    element={
                      <CanAccess
                        resource="products"
                        action="clone"
                        fallback={<Unauthorized />}
                      >
                        <CloneProduct />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/edit"
                    element={
                      <CanAccess
                        resource="products"
                        action="edit"
                        fallback={<Unauthorized />}
                      >
                        <EditProduct />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id"
                    element={
                      <CanAccess
                        resource="products"
                        action="show"
                        fallback={<Unauthorized />}
                      >
                        <ShowProduct />
                      </CanAccess>
                    }
                  />
                </Route>
                <Route path="product-attributes">
                  <Route
                    index
                    element={
                      <CanAccess
                        resource="product-attributes"
                        action="list"
                        fallback={<Unauthorized />}
                      >
                        <ListProductAttribute />
                      </CanAccess>
                    }
                  />
                  <Route
                    path="create"
                    element={
                      <CanAccess
                        resource="product-attributes"
                        action="create"
                        fallback={<Unauthorized />}
                      >
                        <CreateProductAttribute />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/clone"
                    element={
                      <CanAccess
                        resource="product-attributes"
                        action="clone"
                        fallback={<Unauthorized />}
                      >
                        <CloneProductAttribute />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/edit"
                    element={
                      <CanAccess
                        resource="product-attributes"
                        action="edit"
                        fallback={<Unauthorized />}
                      >
                        <EditProductAttribute />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id"
                    element={
                      <CanAccess
                        resource="product-attributes"
                        action="show"
                        fallback={<Unauthorized />}
                      >
                        <ShowProductAttribute />
                      </CanAccess>
                    }
                  />
                </Route>
                <Route path="users">
                  <Route
                    index
                    element={
                      <CanAccess
                        resource="users"
                        action="list"
                        fallback={<Unauthorized />}
                      >
                        <ListUser />
                      </CanAccess>
                    }
                  />
                  <Route
                    path="create"
                    element={
                      <CanAccess
                        resource="users"
                        action="create"
                        fallback={<Unauthorized />}
                      >
                        <CreateUser />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/clone"
                    element={
                      <CanAccess
                        resource="users"
                        action="clone"
                        fallback={<Unauthorized />}
                      >
                        <CloneUser />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/edit"
                    element={
                      <CanAccess
                        resource="users"
                        action="edit"
                        fallback={<Unauthorized />}
                      >
                        <EditUser />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id"
                    element={
                      <CanAccess
                        resource="users"
                        action="show"
                        fallback={<Unauthorized />}
                      >
                        <ShowUser />
                      </CanAccess>
                    }
                  />
                </Route>
                <Route path="roles">
                  <Route
                    index
                    element={
                      <CanAccess
                        resource="roles"
                        action="list"
                        fallback={<Unauthorized />}
                      >
                        <ListRole />
                      </CanAccess>
                    }
                  />
                  <Route
                    path="create"
                    element={
                      <CanAccess
                        resource="roles"
                        action="create"
                        fallback={<Unauthorized />}
                      >
                        <CreateRole />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/clone"
                    element={
                      <CanAccess
                        resource="roles"
                        action="clone"
                        fallback={<Unauthorized />}
                      >
                        <CloneRole />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/edit"
                    element={
                      <CanAccess
                        resource="roles"
                        action="edit"
                        fallback={<Unauthorized />}
                      >
                        <EditRole />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id"
                    element={
                      <CanAccess
                        resource="roles"
                        action="show"
                        fallback={<Unauthorized />}
                      >
                        <ShowRole />
                      </CanAccess>
                    }
                  />
                </Route>
                <Route path="roleclaims">
                  <Route
                    index
                    element={
                      <CanAccess
                        resource="roleclaims"
                        action="list"
                        fallback={<Unauthorized />}
                      >
                        <ListRoleClaim />
                      </CanAccess>
                    }
                  />
                  <Route
                    path="create"
                    element={
                      <CanAccess
                        resource="roleclaims"
                        action="create"
                        fallback={<Unauthorized />}
                      >
                        <CreateRoleClaim />
                      </CanAccess>
                    }
                  />

                  <Route
                    path=":id/clone"
                    element={
                      <CanAccess
                        resource="roleclaims"
                        action="clone"
                        fallback={<Unauthorized />}
                      >
                        <CloneRoleClaim />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id/edit"
                    element={
                      <CanAccess
                        resource="roleclaims"
                        action="edit"
                        fallback={<Unauthorized />}
                      >
                        <EditRoleClaim />
                      </CanAccess>
                    }
                  />
                  <Route
                    path=":id"
                    element={
                      <CanAccess
                        resource="roleclaims"
                        action="show"
                        fallback={<Unauthorized />}
                      >
                        <ShowRoleClaim />
                      </CanAccess>
                    }
                  />
                </Route>
              </Route>
              <Route
                element={
                  <Authenticated key="auth-pages" fallback={<Outlet />}>
                    <NavigateToResource />
                  </Authenticated>
                }
              >
                <Route
                  path="/login"
                  element={
                    <AuthPage
                      type="login"
                      title={
                        <ThemedTitleV2
                          icon={
                            <ImageField
                              value="https://static.vietbank.com.vn/web/vietbank-logo.png"
                              title="Vietbank Logo"
                              style={{ width: 30, height: 30 }}
                            />
                          }
                          text="Vietbank Admin"
                          collapsed={false}
                        />
                      }
                      forgotPasswordLink={false}
                      registerLink={false}
                      formProps={{
                        initialValues: {
                          email: "",
                          password: "",
                        },
                      }}
                    />
                  }
                />
              </Route>
              <Route
                element={
                  <Authenticated key="catch-all">
                    <ThemedLayoutV2>
                      <Outlet />
                    </ThemedLayoutV2>
                  </Authenticated>
                }
              >
                <Route path="*" element={<ErrorComponent />} />
              </Route>
            </Routes>
          </Refine>
        </AntdApp>
      </ConfigProvider>
    </BrowserRouter>
  );
};

export default App;
