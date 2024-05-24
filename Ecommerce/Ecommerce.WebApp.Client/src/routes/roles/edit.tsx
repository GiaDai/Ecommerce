import { Edit, useForm } from "@refinedev/antd";
import { Role } from "./types";
import { Form, Input } from "antd";

export const EditRole = () => {
    const { formProps, saveButtonProps } = useForm<Role>({
        redirect: "edit",
    });
    return (
        <Edit resource="roles" title="Edit Role" saveButtonProps={saveButtonProps} >
            <Form {...formProps} layout="vertical">
                <Form.Item label="ID" name="Id" hidden>
                    <Input />
                </Form.Item>
                <Form.Item label="Name" name="Name">
                    <Input />
                </Form.Item>
            </Form>
        </Edit>
    );
}