import {
  useTable,
  List,
  ShowButton,
  EditButton,
  DeleteButton,
  getDefaultSortOrder,
  DateField,
  FilterDropdown,
  useSelect,
  CloneButton,
} from "@refinedev/antd";
import { Table, Space, Input, DatePicker, Select } from "antd";
import { IProductAttribute } from "./types";
import { getDefaultFilter, useMany, CanAccess } from "@refinedev/core";
import React from "react";
import { IUser } from "@routes/identity/users";
export const ListProductAttribute = () => {
  const { tableProps, sorters, filters } = useTable<IProductAttribute>({
    resource: "product-attributes",
    pagination: { current: 1, pageSize: 10 },
    sorters: { initial: [{ field: "Id", order: "desc" }] },
  });
  const { data: usersCreateBy, isLoading: isLoadingCreateBy } = useMany<IUser>({
    resource: "users",
    ids: [...new Set(tableProps?.dataSource?.map((user) => user.CreatedBy))],
  });

  const { selectProps } = useSelect({
    resource: "users",
    optionLabel: "UserName",
    optionValue: "Id",
    defaultValue: getDefaultFilter("CreatedBy", filters, "eq"),
  });

  const [selectedRowKeys, setSelectedRowKeys] = React.useState<React.Key[]>([]);

  const rowSelection = {
    selectedRowKeys,
    onChange: (selectedRowKeys: React.Key[]) => {
      setSelectedRowKeys(selectedRowKeys);
    },
  };

  return (
    <List resource="product-attributes">
      <Table {...tableProps} rowKey="Id" rowSelection={rowSelection}>
        <Table.Column
          dataIndex="Id"
          title="ID"
          width={50}
          sorter
          defaultSortOrder={getDefaultSortOrder("Id", sorters)}
        />
        <Table.Column
          dataIndex="Name"
          title="Name"
          sorter
          defaultSortOrder={getDefaultSortOrder("Name", sorters)}
          defaultFilteredValue={getDefaultFilter("Name", filters)}
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <Input placeholder="Search Name" />
            </FilterDropdown>
          )}
        />
        <Table.Column dataIndex="Description" title="Description" sorter />
        <Table.Column
          dataIndex="CreatedBy"
          title="Created By"
          width={150}
          render={(value) => {
            if (isLoadingCreateBy) {
              return "Loading...";
            }
            return (
              usersCreateBy?.data?.find((role) => role.Id == value)?.UserName ??
              "Not Found"
            );
          }}
          filterDropdown={(props) => (
            <FilterDropdown
              {...props}
              mapValue={(selectedKey) => String(selectedKey)}
            >
              <Select style={{ minWidth: 200 }} {...selectProps} />
            </FilterDropdown>
          )}
        />
        <Table.Column
          dataIndex="Created"
          title="Created At"
          width={180}
          render={(value) => <DateField format="LLL" value={value} />}
          defaultFilteredValue={getDefaultFilter("Created", filters, "between")}
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <DatePicker.RangePicker />
            </FilterDropdown>
          )}
          sorter
          defaultSortOrder={getDefaultSortOrder("Created", sorters)}
        />
        <Table.Column
          title="Actions"
          width={120}
          render={(_, record: IProductAttribute) => (
            <Space>
              {/* We'll use the `EditButton` and `ShowButton` to manage navigation easily */}
              <ShowButton hideText size="small" recordItemId={record.Id} />
              <EditButton hideText size="small" recordItemId={record.Id} />
              <DeleteButton hideText size="small" recordItemId={record.Id} />
              <CanAccess resource="products" action="clone">
                <CloneButton hideText size="small" recordItemId={record.Id} />
              </CanAccess>
            </Space>
          )}
        />
      </Table>
    </List>
  );
};
