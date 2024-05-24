import { useShow } from "@refinedev/core";
import { RoleClaim } from "./types";
import { Show, TagField, TextField } from "@refinedev/antd";
import { Typography } from "antd";
export const ShowRoleClaim = () => {
  const {
    queryResult: { isError, isLoading, data },
  } = useShow<RoleClaim>();
  if (isError) return <div>Error</div>;
  return (
    <Show isLoading={isLoading} canDelete>
      <Typography.Title level={5}>Resource</Typography.Title>
      <TextField value={data?.data?.ClaimType} />

      <Typography.Title level={5}>Actions</Typography.Title>
      {data?.data?.ClaimValue &&
        data?.data?.ClaimValue.map((claim, index) => (
          <TagField key={index} value={claim} />
        ))}
    </Show>
  );
};
