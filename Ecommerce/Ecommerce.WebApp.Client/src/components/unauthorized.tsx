import React from "react";
import { useGo } from "@refinedev/core";
import { Button, Result } from "antd";

export const Unauthorized: React.FC = () => {
  const go = useGo();
  const handleBackHome = () => {
    go({
      to: {
        resource: "dashboard",
        action: "list",
      },
    });
  };
  return (
    <Result
      status="403"
      title="403"
      subTitle="Sorry, you are not authorized to access this page."
      extra={
        <Button onClick={handleBackHome} type="primary">
          Back Home
        </Button>
      }
    />
  );
};
