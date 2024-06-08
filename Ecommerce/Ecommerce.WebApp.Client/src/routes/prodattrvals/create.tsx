import { useForm, Create, useSelect } from "@refinedev/antd";
import { Checkbox, Form, Input, InputNumber, Select } from "antd";
import { IProdAttrVal } from "./types";
import React from "react";
import { AttributeValueTypeIdData } from "./dummy";
export const CreateProdAttrVal: React.FC = () => {
    const { formProps, saveButtonProps } = useForm<IProdAttrVal>({
        redirect: "list",
    });
    const { selectProps: selectPropsProdAttrMap } = useSelect({
        resource: "product-attribute-mappings",
        optionLabel: "Id",
        optionValue: "Id",
        pagination: {
            pageSize: 99999,
            current: 1,
        }
    });

    return (
        <Create
            resource="product-attribute-mappings"
            saveButtonProps={saveButtonProps}
        >
            <Form {...formProps} layout="vertical">
                <Form.Item<IProdAttrVal>
                    label="Product Attribute Mapping"
                    name="ProductAttributeMappingId"
                    rules={[
                        {
                            required: true,
                            message: "Please input your product id!",
                        },
                    ]}
                >
                    <Select {...selectPropsProdAttrMap} />
                </Form.Item>
                <Form.Item<IProdAttrVal>
                    label="Attribute value type"
                    name="AttributeValueTypeId"
                    rules={[
                        {
                            required: true,
                            message: "Please input your value!",
                        },
                    ]}

                >
                    <Select options={AttributeValueTypeIdData} />
                </Form.Item>
                <Form.Item<IProdAttrVal>
                    label="Name"
                    name="Name"
                    rules={[
                        {
                            required: true,
                            message: "Please input your name!",
                        },
                    ]}
                >
                    <Input />
                </Form.Item>
                <Form.Item<IProdAttrVal>
                    label="Price Adjustment"
                    name="PriceAdjustment"
                    rules={[
                        {
                            required: true,
                            message: "Please input your value!",
                        },
                    ]}
                >
                    <InputNumber />
                </Form.Item>
                <Form.Item<IProdAttrVal> name="PriceAdjustmentUsePercentage" valuePropName="checked">
                    <Checkbox>Price adjustment. Use percentage</Checkbox>
                </Form.Item>
                <Form.Item<IProdAttrVal>
                    label="Weight Adjustment"
                    name="WeightAdjustment"
                    rules={[
                        {
                            required: true,
                            message: "Please input your value!",
                        },
                    ]}
                >
                    <InputNumber />
                </Form.Item>
                <Form.Item<IProdAttrVal>
                    label="Cost"
                    name="Cost"
                    rules={[
                        {
                            required: true,
                            message: "Please input your value!",
                        },
                    ]}
                >
                    <InputNumber />
                </Form.Item>
                <Form.Item<IProdAttrVal> name="IsPreSelected" valuePropName="checked">
                    <Checkbox>Is PreSelected</Checkbox>
                </Form.Item>
                <Form.Item
                    label="Display Order"
                    name="DisplayOrder"
                >
                    <InputNumber defaultValue={1} />
                </Form.Item>
            </Form>
        </Create>
    )
}