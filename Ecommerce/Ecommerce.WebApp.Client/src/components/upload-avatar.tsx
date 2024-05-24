import React, { useState } from "react";
import { PlusOutlined } from "@ant-design/icons";
import { Image, message, Upload } from "antd";
import type { GetProp, UploadFile, UploadProps } from "antd";
import { dataProvider } from "@providers/data-provider";

type FileType = Parameters<GetProp<UploadProps, "beforeUpload">>[0];
interface UploadAvatarProps {
  setUrlAvatar: (avatarProps: AvatarProps) => void;
  publicId?: string;
  url?: string;
}

export type AvatarProps = {
  PublicId: string;
  Url: string;
};

const getBase64 = (file: FileType): Promise<string> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result as string);
    reader.onerror = (error) => reject(error);
  });

const beforeUpload = (file: FileType) => {
  const isJpgOrPng = file.type === "image/jpeg" || file.type === "image/png";
  if (!isJpgOrPng) {
    message.error("You can only upload JPG/PNG file!");
  }
  const isLt2M = file.size / 1024 / 1024 < 2;
  if (!isLt2M) {
    message.error("Image must smaller than 2MB!");
  }
  return isJpgOrPng && isLt2M;
};

export const UploadAvatar: React.FC<UploadAvatarProps> = ({
  setUrlAvatar,
  publicId,
  url,
}) => {
  const [previewOpen, setPreviewOpen] = useState(false);
  const [previewImage, setPreviewImage] = useState<string>();

  const [fileList, setFileList] = useState<UploadFile[]>([]);

  React.useEffect(() => {
    if (url && publicId) {
      setFileList([
        {
          uid: publicId,
          name: "image.png",
          status: "done",
          url,
        },
      ]);
    }
  }, [url, publicId]);
  const handleChange: UploadProps["onChange"] = ({ fileList: newFileList }) => {
    setFileList(newFileList);
    const file = newFileList[newFileList.length - 1];
    if (file.status === "done" && file.response) {
      var avatarResponse = file.response as AvatarProps;
      setUrlAvatar(avatarResponse);
    }
  };

  const uploadButton = (
    <button style={{ border: 0, background: "none" }} type="button">
      <PlusOutlined
        onPointerOverCapture={undefined}
        onPointerMoveCapture={undefined}
      />
      <div style={{ marginTop: 8 }}>Upload</div>
    </button>
  );

  const handlePreview = async (file: UploadFile) => {
    if (!file.url && !file.preview) {
      file.preview = await getBase64(file.originFileObj as FileType);
    }

    setPreviewImage(file.url || (file.preview as string));
    setPreviewOpen(true);
  };

  const handleOnRemove = async (file: UploadFile) => {
    var file = fileList[0];
    console.log(file);
    if (file.response) {
      var avatarResponse = file.response as AvatarProps;
      await dataProvider.deleteFile(avatarResponse.PublicId);
      setUrlAvatar({ PublicId: "", Url: "" });
    } else if (file.uid && file.url) {
      await dataProvider.deleteFile(file.uid);
      setUrlAvatar({ PublicId: "", Url: "" });
    }
    setFileList(fileList.filter((f) => f.uid !== file.uid));
  };

  return (
    <>
      <Upload
        action="/api/file"
        listType="picture-circle"
        fileList={fileList}
        onPreview={handlePreview}
        onChange={handleChange}
        beforeUpload={beforeUpload}
        onRemove={handleOnRemove}
      >
        {fileList.length >= 8 ? null : uploadButton}
      </Upload>
      {previewImage && (
        <Image
          wrapperStyle={{ display: "none" }}
          preview={{
            visible: previewOpen,
            onVisibleChange: (visible) => setPreviewOpen(visible),
            afterOpenChange: (visible) => !visible && setPreviewImage(""),
          }}
          src={previewImage}
        />
      )}
    </>
  );
};
