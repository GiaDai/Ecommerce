import type { IResourceItem } from "@refinedev/core";

import {
  DashboardOutlined,
  TeamOutlined,
  UserOutlined,
  UserSwitchOutlined,
} from "@ant-design/icons";

export const resources: IResourceItem[] = [
  {
    name: "dashboard",
    list: "/",
    meta: {
      label: "Dashboard",
      icon: <DashboardOutlined />,
    },
  },
  {
    name: "products",
    list: "/products",
    create: "/products/create",
    clone: "/products/:id/clone",
    edit: "/products/:id/edit",
    show: "/products/:id",
    meta: {
      canDelete: true,
      label: "Products",
      icon: <UserOutlined />,
    },
  },
  {
    name: "product-attributes",
    list: "/product-attributes",
    create: "/product-attributes/create",
    clone: "/product-attributes/:id/clone",
    edit: "/product-attributes/:id/edit",
    show: "/product-attributes/:id",
    meta: {
      canDelete: true,
      label: "Product Attributes",
      icon: <UserOutlined />,
    },
  },
  {
    name: "users",
    list: "/users",
    create: "/users/create",
    clone: "/users/:id/clone",
    edit: "/users/:id/edit",
    show: "/users/:id",
    meta: {
      canDelete: true,
      label: "Users",
      icon: <UserOutlined />,
    },
  },
  {
    name: "roles",
    list: "/roles",
    create: "/roles/create",
    clone: "/roles/:id/clone",
    edit: "/roles/:id/edit",
    show: "/roles/:id",
    meta: {
      canDelete: true,
      icon: <UserSwitchOutlined />,
    },
  },
  {
    name: "roleclaims",
    identifier: "data-roleclaims",
    list: "/roleclaims",
    create: "/roleclaims/create",
    clone: "/roleclaims/:id/clone",
    edit: "/roleclaims/:id/edit",
    show: "/roleclaims/:id",
    meta: {
      canDelete: true,
      icon: <TeamOutlined />,
    },
  },
];
