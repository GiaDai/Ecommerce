import { useSelect, Edit } from "@refinedev/antd";
import { Checkbox, Form, Input, InputNumber, Select } from "antd";
import { IProductAttributeMapping } from "./types";
import { AttributeControlTypeIdData } from "./dummy";

interface EditProductAttributeMappingProps {
    formProps: any;
    saveButtonProps: any;
}

export const EditProductAttributeMapping: React.FC<EditProductAttributeMappingProps> = ({ formProps, saveButtonProps }) => {

    const { selectProps: selectPropsProduct } = useSelect({
        resource: "products",
        optionLabel: "Name",
        optionValue: "Id",
        pagination: {
            pageSize: 10,
            current: 1,
        }
    });

    const { selectProps: selectPropsProdAttr } = useSelect({
        resource: "product-attributes",
        optionLabel: "Name",
        optionValue: "Id",
        pagination: {
            pageSize: 10,
            current: 1,
        }
    });

    return (
        <Edit
            resource="product-attribute-mappings"
            saveButtonProps={saveButtonProps}
            breadcrumb={false}
            goBack={false}

        >
            <Form {...formProps} layout="vertical">
                <Form.Item label="ID" name="Id" hidden>
                    <Input />
                </Form.Item>
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
        </Edit>
    );
};