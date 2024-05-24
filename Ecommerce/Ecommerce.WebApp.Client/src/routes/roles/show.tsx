import { useShow } from "@refinedev/core";
import { Role } from "./types";
import { Show, TextField } from "@refinedev/antd";
import { Typography } from "antd";

export const ShowRole = () => {
    const {
        queryResult: { isError, isLoading, data },
    } = useShow<Role>();
    if (isError) return <div>Error</div>;
    return (
        <Show isLoading={isLoading} >
            <Typography.Title level={5}>Name</Typography.Title>
            <TextField value={data?.data?.Name} />
        </Show>
    )
};