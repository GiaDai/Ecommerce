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
import { IProductAttributeMapping } from "./types";
import {
    useMany,
} from "@refinedev/core";
import React from "react";
import { PaginationTotal } from "@components/pagination-total";
import { IUser } from "@routes/identity/users";
import { IProduct, IProductAttribute } from "..";
import { AttributeControlTypeIdData } from "./dummy";
export const ListProductAttributeMapping = () => {
    const { tableProps, sorters } = useTable<IProductAttributeMapping>({
        resource: "product-attribute-mappings",
        pagination: { current: 1, pageSize: 10 },
        sorters: { initial: [{ field: "Id", order: "desc" }] },
    });

    const { data: usersCreateBy, isLoading: isLoadingCreateBy } = useMany<IUser>({
        resource: "users",
        ids: [...new Set(tableProps?.dataSource?.map((user) => user.CreatedBy))],
    });

    const { data: products, isLoading: isLoadingProduct } = useMany<IProduct>({
        resource: "products",
        ids: [...new Set(tableProps?.dataSource?.map((product) => product.ProductId))],
    });

    const { data: productAttrs, isLoading: isLoadingProductAttr } = useMany<IProductAttribute>({
        resource: "product-attributes",
        ids: [...new Set(tableProps?.dataSource?.map((productAttr) => productAttr.ProductAttributeId))],
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
                <Table.Column<IProductAttributeMapping>
                    dataIndex="Id"
                    title="ID"
                    sorter
                    width={60}
                    defaultSortOrder={getDefaultSortOrder("Id", sorters)}
                />
                <Table.Column<IProductAttributeMapping>
                    dataIndex="ProductId"
                    title="Product"
                    sorter
                    defaultSortOrder={getDefaultSortOrder("ProductId", sorters)}
                    render={(value) => {
                        if (isLoadingProduct) {
                            return "Loading...";
                        }
                        return (
                            products?.data?.find((product) => product.Id == value)?.Name ??
                            "Not Found"
                        );
                    }}
                />
                <Table.Column<IProductAttributeMapping>
                    dataIndex="ProductAttributeId"
                    title="Attribute"
                    sorter
                    defaultSortOrder={getDefaultSortOrder("ProductAttributeId", sorters)}
                    render={(value) => {
                        if (isLoadingProductAttr) {
                            return "Loading...";
                        }
                        return (
                            productAttrs?.data?.find((productAttr) => productAttr.Id == value)?.Name ??
                            "Not Found"
                        );
                    }}
                />
                <Table.Column<IProductAttributeMapping>
                    dataIndex="TextPrompt"
                    title="Text Prompt"
                    sorter
                />
                <Table.Column<IProductAttributeMapping>
                    dataIndex="AttributeControlTypeId"
                    title="Control Type"
                    sorter
                    render={(value) => {
                        return (
                            AttributeControlTypeIdData.find((attr) => attr.value == value)?.label ?? "Not Found"
                        )
                    }}
                />
                <Table.Column<IProductAttributeMapping>
                    dataIndex="IsRequired"
                    title="Is Required"
                    sorter
                    render={(value: boolean) => {
                        return (
                            <Tag color={value ? "green" : "red"}>
                                {value ? "Required" : "Not Required"}
                            </Tag>
                        )
                    }}


                />
                <Table.Column
                    dataIndex="CreatedBy"
                    title="Created By"
                    render={(value) => {
                        if (isLoadingCreateBy) {
                            return "Loading...";
                        }
                        return (
                            usersCreateBy?.data?.find((role) => role.Id == value)?.UserName ??
                            "Not Found"
                        );
                    }}

                />
                <Table.Column
                    title="Actions"
                    width={150}
                    render={(_, record: IProductAttributeMapping) => (
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