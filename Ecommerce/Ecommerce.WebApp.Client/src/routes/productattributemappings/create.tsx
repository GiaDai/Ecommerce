import { useForm, Create } from "@refinedev/antd";
import { Form, Input } from "antd";
import { IProductAttributeMapping } from "./types";
import React from "react";

type IProductAttributeMappingProps = {
  ProductId: number;
};

export const CreateProductAttributeMapping: React.FC<
  IProductAttributeMappingProps
> = ({ ProductId }) => {
  const { formProps, saveButtonProps } = useForm<IProductAttributeMapping>({
    redirect: "edit",
  });
  return (
    <Create
      resource="product-attribute-mappings"
      saveButtonProps={saveButtonProps}
    >
      <Form {...formProps} layout="vertical">
        <Form.Item
          hidden
          label="Product Id"
          name="ProductId"
          initialValue={ProductId}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Product Attribute Id"
          name="ProductAttributeId"
          rules={[
            {
              required: true,
              message: "Please input your product attribute id!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Text Prompt"
          name="TextPrompt"
          rules={[
            { required: true, message: "Please input your text prompt!" },
            {
              min: 3,
              message: "Text prompt must be at least 3 characters long!",
            },
            {
              max: 255,
              message: "Text prompt must be at most 255 characters long!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Is Required"
          name="IsRequired"
          rules={[
            { required: true, message: "Please input your is required!" },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Attribute Control Type Id"
          name="AttributeControlTypeId"
          rules={[
            {
              required: true,
              message: "Please input your attribute control type id!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Display Order"
          name="DisplayOrder"
          rules={[
            { required: true, message: "Please input your display order!" },
          ]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Create>
  );
};
