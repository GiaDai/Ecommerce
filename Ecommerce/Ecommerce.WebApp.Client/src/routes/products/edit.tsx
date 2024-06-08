import { useForm, Edit } from "@refinedev/antd";
import { IProduct } from "./types";
import { Card, Col, Form, Input, InputNumber, Row } from "antd";
import { useParams } from "react-router-dom";
import { ListProductAttributeMapping } from "@routes/productattributemappings";
export const EditProduct = () => {
  const { id } = useParams<{ id: string }>();
  const { formProps, saveButtonProps } = useForm<IProduct>({
    redirect: "show",
  });
  return (
    <Row>
      <Col span={12}>
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
            <Form.Item label="Barcode" name="Barcode">
              <Input />
            </Form.Item>
            <Form.Item label="Description" name="Description">
              <Input.TextArea rows={4} />
            </Form.Item>
            <Form.Item label="Rate" name="Rate">
              <Input />
            </Form.Item>
            <Form.Item label="Price" name="Price">
              <InputNumber
                style={{ width: "100%" }}
                accept="number"
                defaultValue={0}
                required={true}
              />
            </Form.Item>
          </Form>

        </Edit>
      </Col>
      <Col span={24}>
        <Card style={{ marginTop: 20 }}>
          <ListProductAttributeMapping ProductId={parseInt(id ?? '')} />
        </Card>
      </Col>
    </Row>
  );
};
