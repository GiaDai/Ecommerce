import { UploadOutlined } from "@ant-design/icons";
import { Create, useForm } from "@refinedev/antd";
import {
  Button,
  Col,
  Form,
  Input,
  InputNumber,
  Radio,
  Row,
  Select,
  Upload,
  UploadFile,
  UploadProps,
  message,
} from "antd";
import { useEffect, useState } from "react";
import { IInvestor } from "./types";
import { categoryDummy, delayDummy } from "./dummy";
const urlUpload =
  "https://webvietbank-api.k8s-prod.vietbank.com.vn/api/files/congbothongtin";

const formItemLayout = { labelCol: { span: 6 }, wrapperCol: { span: 20 } };
export const CreateInvestor = () => {
  const [height, setHeight] = useState(window.innerHeight);
  const [urlFile, setUrlFile] = useState<string>("");
  const { formProps, saveButtonProps } = useForm<IInvestor>({
    redirect: "edit",
  });
  useEffect(() => {
    const handleResize = () => setHeight(window.innerHeight);
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);
  const props: UploadProps = {
    name: "file",
    action: urlUpload,
    headers: {
      // authorization: "authorization-text",
    },
    onChange(info) {
      if (info.file.status !== "uploading") {
        console.log(info.file, info.fileList);
      }
      if (info.file.status === "done") {
        if (info.file.response.files[0]) {
          let urlResponse = info.file.response.files[0].url;
          formProps.form?.setFieldsValue({ FileUrl: urlResponse });
          setUrlFile(urlResponse);
        }
        message.success(`${info.file.name} file uploaded successfully`);
      } else if (info.file.status === "error") {
        message.error(`${info.file.name} file upload failed.`);
      }
    },
    onRemove: (file: UploadFile) => {
      var fileDelete = file.response.files[0];
      if (fileDelete) {
        let nameFile = fileDelete.name;
        fetch(`${urlUpload}/${nameFile}`, {
          method: "DELETE",
        })
          .then(() => {
            message.success(`${nameFile} file removed successfully`);
            setUrlFile("");
            formProps.form?.setFieldsValue({ FileUrl: "" });
          })
          .catch((error) => {
            message.error(`${nameFile} file remove failed.`);
            console.error("Error:", error);
          });

        return true;
      }
      message.error(`${file.name} file remove failed.`);
      return false;
    },
    progress: {
      strokeColor: {
        "0%": "#108ee9",
        "100%": "#87d068",
      },
      strokeWidth: 3,
      format: (percent) => percent && `${parseFloat(percent.toFixed(2))}%`,
    },
  };

  return (
    <Row gutter={20}>
      <Col span={12}>
        <Create
          resource="investors"
          title="Thêm mới nhà đầu tư"
          saveButtonProps={saveButtonProps}
        >
          <Form {...formItemLayout} {...formProps} layout="horizontal">
            <Form.Item
              name="Title"
              label="Tiêu đề"
              rules={[
                {
                  required: true,
                  message: "Please input your Title!",
                },
              ]}
            >
              <Input.TextArea rows={4} />
            </Form.Item>
            <Form.Item
              name="Category"
              label="Danh mục"
              rules={[
                {
                  required: true,
                  message: "Please input your Category!",
                },
              ]}
            >
              <Select options={categoryDummy}></Select>
            </Form.Item>
            <Form.Item
              name="Delay"
              label="Thời gian chờ (s)"
              rules={[
                {
                  required: true,
                  message: "Please input your Delay!",
                },
              ]}
            >
              <Radio.Group options={delayDummy}></Radio.Group>
            </Form.Item>
            <Form.Item
              name="Year"
              label="Năm"
              rules={[
                {
                  required: true,
                  message: "Please input your Year!",
                },
              ]}
            >
              <InputNumber />
            </Form.Item>
            <Form.Item name="Quarter" label="Quý">
              <InputNumber />
            </Form.Item>
            <Form.Item
              name="Month"
              label="Tháng"
              rules={[
                {
                  required: true,
                  message: "Please input your Month!",
                },
              ]}
            >
              <InputNumber />
            </Form.Item>
            <Form.Item label="Tải file">
              <Upload {...props}>
                <Button icon={<UploadOutlined />}>Click to Upload</Button>
              </Upload>
            </Form.Item>
            <Form.Item
              name="FileUrl"
              label="File URL"
              rules={[
                {
                  required: true,
                  message: "Please input your FileUrl!",
                },
              ]}
            >
              <Input.TextArea rows={3} value={urlFile} />
            </Form.Item>
          </Form>
        </Create>
        ;
      </Col>
      <Col span={12}>
        {urlFile && (
          <iframe src={urlFile} width="100%" height={height - 50}></iframe>
        )}
      </Col>
    </Row>
  );
};
