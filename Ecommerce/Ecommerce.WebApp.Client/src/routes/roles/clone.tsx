import { Create, useForm } from "@refinedev/antd";
import { Role } from "./types";
import { Form, Input } from "antd";

export const CloneRole = () => {
  const { formProps, saveButtonProps } = useForm<Role>({
    redirect: "list",
  });
  return (
    <Create
      resource="roles"
      title="Clone Role"
      saveButtonProps={saveButtonProps}
    >
      <Form {...formProps} layout="vertical">
        <Form.Item label="Name" name="Name">
          <Input />
        </Form.Item>
      </Form>
    </Create>
  );
};
