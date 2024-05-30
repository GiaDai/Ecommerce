import { useForm, Create } from "@refinedev/antd";
import { Form, Input } from "antd";
import { IProductAttribute } from "./types";
export const CreateProductAttribute = () => {
  const { formProps, saveButtonProps } = useForm<IProductAttribute>({
    redirect: "edit",
  });
  return (
    <Create resource="product-attributes" saveButtonProps={saveButtonProps}>
      <Form {...formProps} layout="vertical">
        <Form.Item
          label="Name"
          name="Name"
          rules={[
            { required: true, message: "Please input your product name!" },
            {
              min: 3,
              message:
                "Product attribute name must be at least 3 characters long!",
            },
            {
              max: 255,
              message:
                "Product attribute name must be at most 255 characters long!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item label="Description" name="Description">
          <Input.TextArea rows={4} />
        </Form.Item>
      </Form>
    </Create>
  );
};
