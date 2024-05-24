import {
  DeleteButton,
  EditButton,
  FilterDropdown,
  List,
  ShowButton,
  TagField,
  useSelect,
  useTable,
  CloneButton,
} from "@refinedev/antd";
import { RoleClaim } from "./types";
import { Select, Space, Table } from "antd";
import { getDefaultFilter, useMany } from "@refinedev/core";
import { Role } from "@routes/roles/types";
export const ListRoleClaim = () => {
  const { tableProps, filters } = useTable<RoleClaim>({
    resource: "roleclaims",
    pagination: { current: 1, pageSize: 10 },
    sorters: { initial: [{ field: "Id", order: "asc" }] },
  });

  const { data: roles, isLoading } = useMany<Role>({
    resource: "roles",
    ids: tableProps?.dataSource?.map((roleclaim) => roleclaim.RoleId) ?? [],
  });

  const { selectProps } = useSelect({
    resource: "roles",
    optionLabel: "Name",
    optionValue: "Id",
    defaultValue: getDefaultFilter("RoleId", filters, "eq"),
  });

  return (
    <List>
      <Table {...tableProps} rowKey="Id">
        <Table.Column<RoleClaim> dataIndex="Id" title="ID" />
        <Table.Column<RoleClaim>
          dataIndex="RoleId"
          title="Role"
          render={(value) => {
            if (isLoading) {
              return "Loading...";
            }
            return (
              roles?.data?.find((role) => role.Id == value)?.Name ?? "Not Found"
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
          defaultFilteredValue={getDefaultFilter("RoleId", filters, "eq")}
        />
        <Table.Column<RoleClaim> dataIndex="ClaimType" title="Resource" />
        <Table.Column<RoleClaim>
          dataIndex="ClaimValue"
          title="Actions"
          render={(value: string) => (
            <>
              {value.split("#").map((claim, index) => (
                <TagField key={index} value={claim} />
              ))}
            </>
          )}
        />
        <Table.Column
          title="Actions"
          width={150}
          render={(_, record: RoleClaim) => (
            <Space>
              {/* We'll use the `EditButton` and `ShowButton` to manage navigation easily */}
              <ShowButton hideText size="small" recordItemId={record.Id} />
              <EditButton
                hideText
                size="small"
                recordItemId={record.Id}
                accessControl={{ enabled: true, hideIfUnauthorized: false }}
              />
              <DeleteButton hideText size="small" recordItemId={record.Id} />
              <CloneButton
                hideText
                size="small"
                meta={{ authorId: 10 }}
                recordItemId={record.Id}
              />
            </Space>
          )}
        />
      </Table>
    </List>
  );
};
