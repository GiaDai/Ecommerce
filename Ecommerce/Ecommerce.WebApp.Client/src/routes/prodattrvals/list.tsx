import {
    useTable,
    List,
    ShowButton,
    EditButton,
    DeleteButton,
    getDefaultSortOrder,
    CloneButton,
} from "@refinedev/antd";
import { Table, Space, Tag } from "antd";
import {
    useMany,
} from "@refinedev/core";
import React from "react";
import { PaginationTotal } from "@components/pagination-total";
import { IUser } from "@routes/identity/users";
import { IProdAttrVal } from "./types";
import { IProductAttributeMapping } from "@routes/productattributemappings";
import { AttributeValueTypeIdData } from "./dummy";

export const ListProdAttrVals: React.FC = () => {
    const { tableProps, sorters } = useTable<IProdAttrVal>({
        resource: "product-attribute-values",
        pagination: { current: 1, pageSize: 10 },
        sorters: { initial: [{ field: "Id", order: "desc" }] },
    });

    const [selectedRowKeys, setSelectedRowKeys] = React.useState<React.Key[]>([]);
    const rowSelection = {
        selectedRowKeys,
        onChange: (selectedRowKeys: React.Key[]) => {
            setSelectedRowKeys(selectedRowKeys);
        },
    };
    return (
        <List>
            <Table
                {...tableProps}
                rowKey="Id"
                pagination={{
                    ...tableProps.pagination,
                    showTotal: (total) => (
                        <PaginationTotal total={total} entityName="products" />
                    ),
                }}
                rowSelection={rowSelection}
            >
                <Table.Column<IProdAttrVal>
                    dataIndex="Id"
                    title="ID"
                    sorter
                    width={60}
                    defaultSortOrder={getDefaultSortOrder("Id", sorters)}
                />

                <Table.Column<IProdAttrVal>
                    dataIndex="AttributeValueTypeId"
                    title="Control Type"
                    sorter
                    render={(value) => {
                        return (
                            AttributeValueTypeIdData.find((attr) => attr.value == value)?.label ?? "Not Found"
                        )
                    }}
                />
                <Table.Column<IProdAttrVal>
                    dataIndex="Name"
                    title="Name"
                />
                <Table.Column
                    title="Actions"
                    width={150}
                    render={(_, record: IProdAttrVal) => (
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
    )
}