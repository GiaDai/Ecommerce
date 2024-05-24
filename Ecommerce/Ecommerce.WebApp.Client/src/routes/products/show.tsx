import { useShow } from "@refinedev/core";
import { IProduct } from "./types";
import { Show, TextField } from "@refinedev/antd";
import { Typography } from "antd";

export const ShowProduct = () => {
  const {
    queryResult: { isLoading, data },
  } = useShow<IProduct>();

  return (
    <Show isLoading={isLoading}>
      <Typography.Title level={5}>Id</Typography.Title>
      <TextField value={data?.data.Id} />

      <Typography.Title level={5}>Name</Typography.Title>
      <TextField value={data?.data?.Name} />

      <Typography.Title level={5}>Barcode</Typography.Title>
      <TextField value={data?.data?.Barcode} />

      <Typography.Title level={5}>Price</Typography.Title>
      <TextField value={data?.data?.Price} />

      <Typography.Title level={5}>Rate</Typography.Title>
      <TextField value={data?.data?.Rate} />

      <Typography.Title level={5}>Description</Typography.Title>
      <TextField value={data?.data?.Description} />
    </Show>
  );
};
