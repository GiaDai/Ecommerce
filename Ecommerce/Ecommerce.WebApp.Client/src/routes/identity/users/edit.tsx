import { useEffect } from "react";
import { Edit, useForm, useSelect } from "@refinedev/antd";
import { IUser, IUserAvatar } from "./types";
import { Avatar, Checkbox, Col, Form, Input, Row, Select } from "antd";
import React from "react";
import { AvatarProps } from "@components/upload-avatar";
// https://ant.design/components/upload
export const EditUser = () => {
  const [urlAvatar, setUrlAvatar] = React.useState<AvatarProps>();
  const { formProps, saveButtonProps } = useForm<IUser>({
    redirect: "list",
  });

  useEffect(() => {
    const avatarUrl = formProps.form?.getFieldValue("Avatar") as IUserAvatar;
    setUrlAvatar({
      PublicId: avatarUrl?.AvatarUid,
      Url: avatarUrl?.AvatarUrl,
    });
    if (urlAvatar?.PublicId && urlAvatar?.Url) {
      formProps.form?.setFieldValue("Avatar", [
        {
          AvatarUid: urlAvatar?.PublicId,
          AvatarUrl: urlAvatar.Url,
        },
      ]);
    }
  }, [formProps.form, urlAvatar]);

  const { selectProps } = useSelect({
    resource: "roles",
    optionLabel: "Name",
    optionValue: "Id",
  });

  const formItemLayout = { labelCol: { span: 6 }, wrapperCol: { span: 14 } };

  return (
    <Edit resource="users" title="Edit user" saveButtonProps={saveButtonProps}>
      <Form {...formItemLayout} {...formProps} layout="horizontal">
        <Row gutter={20}>
          <Col span={12}>
            <Form.Item label="Id" name="Id" hidden>
              <Input />
            </Form.Item>
            <Form.Item
              label="Tài khoản"
              name="UserName"
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
              label="Quyền"
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
              label="Họ"
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
              label="Tên"
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
                src={`https://documents.vietbank.com.vn/avatar/${formProps.form?.getFieldValue(
                  "Email"
                )}.jpg`}
              />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Edit>
  );
};
