import { useForm, Edit } from "@refinedev/antd";
import { IProductAttribute } from "./types";
import { Form, Input } from "antd";

export const EditProductAttribute = () => {
  const { formProps, saveButtonProps } = useForm<IProductAttribute>({
    redirect: "show",
  });
  return (
    <Edit saveButtonProps={saveButtonProps}>
      <Form {...formProps} layout="vertical">
        <Form.Item label="ID" name="Id" hidden>
          <Input />
        </Form.Item>
        <Form.Item
          label="Name"
          name="Name"
          rules={[
            { required: true, message: "Please input your product name!" },
            {
              min: 3,
              message: "Product name must be at least 3 characters long!",
            },
            {
              max: 255,
              message: "Product name must be at most 255 characters long!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item label="Description" name="Description">
          <Input.TextArea rows={4} />
        </Form.Item>
      </Form>
    </Edit>
  );
};
