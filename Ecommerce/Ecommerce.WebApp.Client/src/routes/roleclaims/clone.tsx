import { useForm, useSelect, Create } from "@refinedev/antd";
import { RoleClaim } from "./types";
import { Form, Input, Select } from "antd";

export const CloneRoleClaim = () => {
  const { formProps, saveButtonProps } = useForm<RoleClaim>({
    redirect: "list",
  });

  const { selectProps } = useSelect({
    resource: "roles",
    optionLabel: "Name",
    optionValue: "Id",
  });
  return (
    <Create
      resource="roleclaims"
      title="Clone Role Claim"
      saveButtonProps={saveButtonProps}
    >
      <Form {...formProps} layout="vertical">
        <Form.Item label="Role" name="RoleId">
          <Select {...selectProps} />
        </Form.Item>
        <Form.Item
          label="Resource"
          name="ClaimType"
          rules={[{ required: true, message: "Please input your ClaimType!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="ClaimValue"
          label="Actions"
          rules={[
            {
              required: true,
              message: "Please select action for this resource!",
              type: "array",
            },
          ]}
        >
          <Select mode="tags" placeholder="Please select favourite colors">
            <Select.Option value="list">list</Select.Option>
            <Select.Option value="create">create</Select.Option>
            <Select.Option value="show">show</Select.Option>
            <Select.Option value="edit">edit</Select.Option>
            <Select.Option value="delete">delete</Select.Option>
          </Select>
        </Form.Item>
      </Form>
    </Create>
  );
};
