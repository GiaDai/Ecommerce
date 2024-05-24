import { Create, useForm, useSelect } from "@refinedev/antd";
import { ICreateUser, IUserLdapInfo } from "./types";
import {
  Form,
  Input,
  Row,
  Col,
  Select,
  Checkbox,
  Space,
  Button,
  Avatar,
} from "antd";
import { dataProvider } from "@providers/data-provider";
import { useNotification } from "@refinedev/core";
import React from "react";
export const CreateUser = () => {
  const { open } = useNotification();
  const [urlAvatar, setUrlAvatar] = React.useState<string>("");
  const { formProps, saveButtonProps } = useForm<ICreateUser>({
    redirect: "edit",
  });

  const { selectProps } = useSelect({
    resource: "roles",
    optionLabel: "Name",
    optionValue: "Id",
  });

  const handleGetUserLdap = async () => {
    try {
      const username = formProps.form?.getFieldValue("UserName");
      const userLdap = await dataProvider.getUserLdap(username);
      const ldap = userLdap.data as IUserLdapInfo;
      let names = ldap.DisplayName.split(" ");
      let firstName = names[0];
      let lastName = names.length > 1 ? names.slice(1).join(" ") : "";
      setUrlAvatar(ldap.EmailAddress);
      formProps.form?.setFieldsValue({
        FirstName: firstName,
        LastName: lastName,
        Email: ldap.EmailAddress,
      });
    } catch (error) {
      open?.({
        type: "error",
        message: (error as Error)?.message,
        undoableTimeout: 5,
      });
      setUrlAvatar("");
      formProps.form?.setFieldsValue({
        FirstName: "",
        LastName: "",
        Email: "",
      });
    }
  };

  const formItemLayout = { labelCol: { span: 6 }, wrapperCol: { span: 14 } };
  return (
    <Create
      resource="roles"
      title="Create User"
      saveButtonProps={saveButtonProps}
    >
      <Form {...formItemLayout} {...formProps} layout="horizontal">
        <Row gutter={20}>
          <Col span={12}>
            <Form.Item
              label="UserName"
              name="UserName"
              rules={[
                {
                  required: true,
                  message: "Please input your FirstName!",
                },
              ]}
            >
              <Space.Compact style={{ width: "100%" }}>
                <Input />
                <Button onClick={handleGetUserLdap} type="primary">
                  Tìm
                </Button>
              </Space.Compact>
            </Form.Item>
            <Form.Item
              label="Role"
              name="RoleId"
              rules={[
                {
                  required: true,
                  message: "Please input your Role!",
                },
              ]}
            >
              <Select {...selectProps} />
            </Form.Item>
            <Form.Item
              label="FirstName"
              name="FirstName"
              rules={[
                {
                  required: true,
                  message: "Please input your FirstName!",
                },
              ]}
            >
              <Input readOnly disabled />
            </Form.Item>
            <Form.Item
              label="LastName"
              name="LastName"
              rules={[
                {
                  required: true,
                  message: "Please input your FirstName!",
                },
              ]}
            >
              <Input readOnly disabled />
            </Form.Item>

            <Form.Item
              label="Email"
              name="Email"
              rules={[
                {
                  type: "email",
                  message: "The input is not valid E-mail!",
                },
                {
                  required: true,
                  message: "Please input your E-mail!",
                },
              ]}
            >
              <Input readOnly disabled />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="EmailConfirmed" valuePropName="checked">
              <Checkbox>Hoạt động</Checkbox>
            </Form.Item>
            <Form.Item>
              <Avatar
                size={{ xs: 24, sm: 32, md: 40, lg: 64, xl: 200, xxl: 200 }}
                src={`https://documents.vietbank.com.vn/avatar/${urlAvatar}.jpg`}
              />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Create>
  );
};
