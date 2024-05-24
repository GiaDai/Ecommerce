import { AutoSaveIndicator } from "@refinedev/core";
import { useForm, Edit, useSelect } from "@refinedev/antd";
import { RoleClaim } from "./types";
import { Form, Input, Select } from "antd";
export const EditRoleClaim = () => {
  const { formProps, saveButtonProps, autoSaveProps } = useForm<RoleClaim>({
    redirect: "show",
  });

  const { selectProps } = useSelect({
    resource: "roles",
    optionLabel: "Name",
    optionValue: "Id",
  });

  return (
    <Edit saveButtonProps={{ ...saveButtonProps }}>
      <AutoSaveIndicator {...autoSaveProps} />
      <Form {...formProps} layout="vertical">
        <Form.Item label="ID" name="Id" hidden>
          <Input />
        </Form.Item>
        <Form.Item label="RoleId" name="RoleId">
          <Select {...selectProps} />
        </Form.Item>
        <Form.Item
          label="ClaimType"
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
    </Edit>
  );
};
