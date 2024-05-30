import { useShow } from "@refinedev/core";
import { IProductAttribute } from "./types";
import { Show, TextField } from "@refinedev/antd";
import { Typography } from "antd";

export const ShowProductAttribute = () => {
  const {
    queryResult: { isLoading, data },
  } = useShow<IProductAttribute>();
  return (
    <Show isLoading={isLoading}>
      <Typography.Title level={5}>Id</Typography.Title>
      <TextField value={data?.data.Id} />

      <Typography.Title level={5}>Name</Typography.Title>
      <TextField value={data?.data?.Name} />

      <Typography.Title level={5}>Description</Typography.Title>
      <TextField value={data?.data?.Description} />
    </Show>
  );
};
