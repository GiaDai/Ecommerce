import React, { useState } from "react";
import { ImportButton, useImport, Create, Breadcrumb } from "@refinedev/antd";
import { Space, Table, Tag } from "antd";
import type { TableProps } from "antd";
import { HttpError } from "@refinedev/core";
import { IProduct } from "./types";

interface IProductError extends IProduct {
  Message: string;
  Success: boolean;
}

export const CreateRangeProduct: React.FC = () => {
  const [importProgress, setImportProgress] = useState({
    processed: 0,
    total: 0,
  });

  const handleSuccess = (successes: any[]) => {
    successes.forEach((success) => {
      const successData = success.response as IProductError[];
      setResponses((prev: IProductError[]) => [...prev, ...successData]);
    });
  };

  const handleError = (errors: any[]) => {
    errors.forEach((error) => {
      const errorRequest = error.request as IProduct[];
      const errorResponse = error.response as HttpError[];
      setResponses((prev) => [
        ...prev,
        ...errorRequest.map((request, index) => ({
          ...request,
          Message: errorResponse[index].message,
          Success: false,
        })),
      ]);
    });
  };

  const [responses, setResponses] = useState<IProductError[]>([]);
  const importProps = useImport<IProductError>({
    resource: "products",
    onFinish: (result) => {
      const { succeeded, errored } = result;

      if (succeeded.length > 0) {
        handleSuccess(succeeded);
      }

      if (errored.length > 0) {
        handleError(errored);
      }
    },
    onProgress: (progress) => {
      setImportProgress({
        processed: progress.processedAmount,
        total: progress.totalAmount,
      });
    },
    paparseOptions: {
      header: false,
    },
    batchSize: 5,
  });

  const columns: TableProps<IProductError>["columns"] = [
    {
      title: "Success",
      dataIndex: "Success",
      key: "Success",
      render: (value) =>
        value ? (
          <Tag color={"green"} key={"loser"}>
            {"Success"}
          </Tag>
        ) : (
          <Tag color={"red"} key={"volcano"}>
            {"Error"}
          </Tag>
        ),
    },
    {
      title: "Name",
      dataIndex: "Name",
      key: "Name",
    },
    {
      title: "Barcode",
      dataIndex: "Barcode",
      key: "Barcode",
    },
    {
      title: "Message",
      dataIndex: "Message",
      key: "Message",
    },
  ];

  return (
    <Create
      title="Create range products"
      breadcrumb={
        <Breadcrumb
          breadcrumbProps={{
            items: [
              {
                title: "Products",
                href: "/products",
              },
              {
                title: "Create range",
              },
            ],
          }}
        />
      }
    >
      <Space style={{ marginBottom: 16 }}>
        <ImportButton {...importProps} accept=".csv" />
        <span>
          {importProgress.processed}/{importProgress.total}
        </span>
      </Space>
      <Table columns={columns} dataSource={responses} />
    </Create>
  );
};
