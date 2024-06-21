import { Products } from "@components/product";
import { Route, Routes } from "react-router-dom";

export const Routing = () => {
  return (
    <Routes>
      <Route path="/" element={<div>Home</div>} />
      <Route path="/:categoryId" element={<Products />} />
    </Routes>
  );
};
