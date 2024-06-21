import { Typography } from "antd";
import MenuHeader from "./menu";
import { CartHeader } from "./cart";
export const Header = () => {
  return (
    <div className="appHeader">
      <MenuHeader />
      <Typography.Title>Aamir Store</Typography.Title>
      <CartHeader />
    </div>
  );
};
