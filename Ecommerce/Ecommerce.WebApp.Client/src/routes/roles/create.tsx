import { Create, useForm } from "@refinedev/antd";
import { Role } from "./types";
import { Form, Input } from "antd";

export const CreateRole = () => {
    const { formProps, saveButtonProps } = useForm<Role>({
        redirect: "edit",
    });
    return (
        <Create resource="roles" title="Create Role" saveButtonProps={saveButtonProps} >
            <Form {...formProps} layout="vertical">
                <Form.Item label="Name" name="Name">
                    <Input />
                </Form.Item>
            </Form>
        </Create>
    );
}