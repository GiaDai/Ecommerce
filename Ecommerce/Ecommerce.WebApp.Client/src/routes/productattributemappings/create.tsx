import { useForm, Create, useSelect } from "@refinedev/antd";
import { FormProps, ButtonProps, Checkbox, Form, Input, InputNumber, Select } from "antd";
import { IProductAttributeMapping } from "./types";
import React from "react";
import { AttributeControlTypeIdData } from "./dummy";

interface CreateProductAttributeMappingProps {
  formProps: FormProps;
  saveButtonProps: ButtonProps;
}

export const CreateProductAttributeMapping: React.FC<CreateProductAttributeMappingProps> = ({ formProps, saveButtonProps }) => {

  const { selectProps: selectPropsProduct } = useSelect({
    resource: "products",
    optionLabel: "Name",
    optionValue: "Id",
    pagination: {
      pageSize: 99999,
      current: 1,
    }
  });

  const { selectProps: selectPropsProdAttr } = useSelect({
    resource: "product-attributes",
    optionLabel: "Name",
    optionValue: "Id",
    pagination: {
      pageSize: 99999,
      current: 1,
    }
  });

  return (
    <Create
      breadcrumb={false}
      goBack={false}
      resource="product-attribute-mappings"
      saveButtonProps={saveButtonProps}
    >
      <Form {...formProps} layout="vertical">
        <Form.Item
          label="Product"
          name="ProductId"
          rules={[
            {
              required: true,
              message: "Please input your product id!",
            },
          ]}
        >
          <Select {...selectPropsProduct} />
        </Form.Item>

        <Form.Item
          label="Product Attribute"
          name="ProductAttributeId"
          rules={[
            {
              required: true,
              message: "Please input your product attribute id!",
            },
          ]}
        >
          <Select {...selectPropsProdAttr} />
        </Form.Item>

        <Form.Item
          label="Text Prompt"
          name="TextPrompt"
        >
          <Input />
        </Form.Item>

        <Form.Item<IProductAttributeMapping> name="IsRequired" valuePropName="checked">
          <Checkbox>Is Required</Checkbox>
        </Form.Item>

        <Form.Item
          label="Control type"
          name="AttributeControlTypeId"
          rules={[{ required: true, message: "Control type is required" }]}
        >
          <Select options={AttributeControlTypeIdData} />
        </Form.Item>

        <Form.Item
          label="Display Order"
          name="DisplayOrder"
        >
          <InputNumber defaultValue={1} />
        </Form.Item>
      </Form>
    </Create>
  );
};
