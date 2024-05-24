import {
  DateField,
  DeleteButton,
  EditButton,
  FilterDropdown,
  List,
  useTable,
} from "@refinedev/antd";
import { IInvestor } from "./types";
import {
  Input,
  InputNumber,
  Radio,
  Select,
  Space,
  Table,
  Typography,
} from "antd";
import { categoryDummy, delayDummy } from "./dummy";
import { getDefaultFilter, useMany } from "@refinedev/core";
import { IUser } from "@routes/identity/users";

const selectProps = {
  options: categoryDummy,
};

export const ListInvestor = () => {
  const { tableProps, filters } = useTable<IInvestor>({
    resource: "investors",
    pagination: { current: 1, pageSize: 10 },
    sorters: { initial: [{ field: "Id", order: "desc" }] },
  });

  const { data: users } = useMany<IUser>({
    resource: "users",
    ids: [...new Set(tableProps?.dataSource?.map((user) => user.CreatedBy))],
  });
  return (
    <List title="Nhà đầu tư">
      <Table {...tableProps} rowKey="Id">
        <Table.Column<IInvestor>
          title="#"
          key="rowNumber"
          render={(_text, _record, index) => index + 1}
        />
        <Table.Column<IInvestor>
          dataIndex="Title"
          title="Tiêu đề"
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <Input placeholder="Tìm" />
            </FilterDropdown>
          )}
          render={(value, record) => {
            return (
              <Space direction="vertical">
                <Typography.Text>{value}</Typography.Text>
                <Space direction="horizontal">
                  <Typography.Text code>
                    {
                      users?.data?.find(
                        (user: IUser) => user.Id === record.CreatedBy
                      )?.UserName
                    }
                  </Typography.Text>
                  <Typography.Text code>
                    <DateField
                      format="DD/MM/YYYY HH:mm:ss"
                      value={record.Created}
                    />{" "}
                  </Typography.Text>
                </Space>
              </Space>
            );
          }}
        />
        <Table.Column<IInvestor>
          dataIndex="Category"
          title="Danh mục"
          render={(value) => {
            // Search top-level items
            let foundItem = categoryDummy.find((item) => item.value === value);
            if (foundItem) {
              return foundItem.label;
            }

            // Search in options
            for (let item of categoryDummy) {
              if (item.options) {
                foundItem = item.options.find(
                  (option) => option.value === value
                );
                if (foundItem) {
                  return foundItem.label;
                }
              }
            }

            // Return undefined if no matching label is found
            return undefined;
          }}
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <Select style={{ minWidth: 200 }} {...selectProps} />
            </FilterDropdown>
          )}
          defaultFilteredValue={getDefaultFilter("Category", filters, "eq")}
        />
        <Table.Column<IInvestor>
          dataIndex="Year"
          title="Năm"
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <InputNumber placeholder="Tìm" />
            </FilterDropdown>
          )}
          defaultFilteredValue={getDefaultFilter("Year", filters, "eq")}
        />
        <Table.Column<IInvestor>
          dataIndex="Quarter"
          title="Quý"
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <InputNumber placeholder="Tìm" />
            </FilterDropdown>
          )}
          defaultFilteredValue={getDefaultFilter("Quarter", filters, "eq")}
        />
        <Table.Column<IInvestor>
          dataIndex="Month"
          title="Tháng"
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <InputNumber placeholder="Tìm" />
            </FilterDropdown>
          )}
          defaultFilteredValue={getDefaultFilter("Month", filters, "eq")}
        />
        <Table.Column<IInvestor>
          dataIndex="Delay"
          title="Độ trễ"
          filterDropdown={(props) => (
            <FilterDropdown {...props}>
              <Radio.Group options={delayDummy}></Radio.Group>
            </FilterDropdown>
          )}
          defaultFilteredValue={getDefaultFilter("Delay", filters, "eq")}
        />
        <Table.Column
          title="Actions"
          width={80}
          render={(_, record: IInvestor) => (
            <Space>
              {/* We'll use the `EditButton` and `ShowButton` to manage navigation easily */}
              <EditButton hideText size="small" recordItemId={record.Id} />
              <DeleteButton hideText size="small" recordItemId={record.Id} />
            </Space>
          )}
        />
      </Table>
    </List>
  );
};
