import React from "react";

import { ThemedLayoutContextProvider } from "@refinedev/antd";

import { Grid, Layout as AntdLayout } from "antd";

export const Layout: React.FC<React.PropsWithChildren> = ({ children }) => {
  const breakpoint = Grid.useBreakpoint();
  const isSmall = typeof breakpoint.sm === "undefined" ? true : breakpoint.sm;

  return (
    <ThemedLayoutContextProvider>
      <AntdLayout hasSider style={{ minHeight: "100vh" }}>
        <AntdLayout>
          <AntdLayout.Content
            style={{
              padding: isSmall ? 32 : 16,
            }}
          >
            {children}
          </AntdLayout.Content>
        </AntdLayout>
      </AntdLayout>
    </ThemedLayoutContextProvider>
  );
};
