import {
  DeleteButton,
  EditButton,
  CloneButton,
  List,
  ShowButton,
  useTable,
} from "@refinedev/antd";
import { Role } from "./types";
import { Space, Table } from "antd";

export const ListRole = () => {
  const { tableProps } = useTable<Role>({
    resource: "roles",
    pagination: { current: 1, pageSize: 10 },
    sorters: { initial: [{ field: "Id", order: "asc" }] },
  });

  return (
    <List>
      <Table {...tableProps} rowKey="Id">
        <Table.Column dataIndex="Id" title="ID" />
        <Table.Column dataIndex="Name" title="Name" />
        <Table.Column dataIndex="NormalizedName" title="NormalizedName" />
        <Table.Column
          title="Actions"
          width={150}
          render={(_, record: Role) => (
            <Space>
              {/* We'll use the `EditButton` and `ShowButton` to manage navigation easily */}
              <ShowButton hideText size="small" recordItemId={record.Id} />
              <EditButton hideText size="small" recordItemId={record.Id} />
              <DeleteButton hideText size="small" recordItemId={record.Id} />
              <CloneButton hideText size="small" recordItemId={record.Id} />
            </Space>
          )}
        />
      </Table>
    </List>
  );
};
