import {
  Badge,
  Button,
  Card,
  Image,
  List,
  message,
  Rate,
  Typography,
  Select,
} from "antd";
import { useEffect, useState } from "react";
import { addToCart, getAllProducts, getProductsByCategory } from "@api/index";
import { useParams } from "react-router-dom";
import { Product } from "./types";

export const Products = () => {
  const [loading, setLoading] = useState(false);
  const param = useParams();
  const [items, setItems] = useState<Product[]>([]);
  const [sortOrder, setSortOrder] = useState("az");
  useEffect(() => {
    setLoading(true);
    (param?.categoryId
      ? getProductsByCategory(param.categoryId)
      : getAllProducts()
    ).then((res) => {
      setItems(res.products);
      setLoading(false);
    });
  }, [param]);

  const getSortedItems = () => {
    const sortedItems = [...items];
    sortedItems.sort((a, b) => {
      const aLowerCaseTitle = a.title.toLowerCase();
      const bLowerCaseTitle = b.title.toLowerCase();

      if (sortOrder === "az") {
        return aLowerCaseTitle > bLowerCaseTitle
          ? 1
          : aLowerCaseTitle === bLowerCaseTitle
          ? 0
          : -1;
      } else if (sortOrder === "za") {
        return aLowerCaseTitle < bLowerCaseTitle
          ? 1
          : aLowerCaseTitle === bLowerCaseTitle
          ? 0
          : -1;
      } else if (sortOrder === "lowHigh") {
        return a.price > b.price ? 1 : a.price === b.price ? 0 : -1;
      } else if (sortOrder === "highLow") {
        return a.price < b.price ? 1 : a.price === b.price ? 0 : -1;
      }

      return 0; // Add a default return statement
    });
    return sortedItems;
  };

  return (
    <div className="productsContainer">
      <div>
        <Typography.Text>View Items Sorted By: </Typography.Text>
        <Select
          onChange={(value) => {
            setSortOrder(value);
          }}
          defaultValue={"az"}
          options={[
            {
              label: "Alphabetically a-z",
              value: "az",
            },
            {
              label: "Alphabetically z-a",
              value: "za",
            },
            {
              label: "Price Low to High",
              value: "lowHigh",
            },
            {
              label: "Price High to Low",
              value: "highLow",
            },
          ]}
        ></Select>
      </div>
      <List
        loading={loading}
        grid={{ column: 3 }}
        renderItem={(product, index) => {
          return (
            <Badge.Ribbon
              className="itemCardBadge"
              text={`${product.discountPercentage}% Off`}
              color="pink"
            >
              <Card
                className="itemCard"
                title={product.title}
                key={index}
                cover={
                  <Image className="itemCardImage" src={product.thumbnail} />
                }
                actions={[
                  <Rate allowHalf disabled value={product.rating} />,
                  <AddToCartButton item={product} />,
                ]}
              >
                <Card.Meta
                  title={
                    <Typography.Paragraph>
                      Price: ${product.price}{" "}
                      <Typography.Text delete type="danger">
                        $
                        {(
                          product.price +
                          (product.price * product.discountPercentage) / 100
                        )
                          .toFixed(2)
                          .toString()}
                      </Typography.Text>
                    </Typography.Paragraph>
                  }
                  description={
                    <Typography.Paragraph
                      ellipsis={{ rows: 2, expandable: true, symbol: "more" }}
                    >
                      {product.description}
                    </Typography.Paragraph>
                  }
                ></Card.Meta>
              </Card>
            </Badge.Ribbon>
          );
        }}
        dataSource={getSortedItems()}
      ></List>
    </div>
  );
};

function AddToCartButton({ item }: { item: Product }) {
  const [loading, setLoading] = useState(false);
  const addProductToCart = () => {
    setLoading(true);
    addToCart(item.id).then(() => {
      message.success(`${item.title} has been added to cart!`);
      setLoading(false);
    });
  };
  return (
    <Button
      type="link"
      onClick={() => {
        addProductToCart();
      }}
      loading={loading}
    >
      Add to Cart
    </Button>
  );
}
