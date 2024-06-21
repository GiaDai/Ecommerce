import "./App.css";
import { App as AntApp } from "antd";
import { BrowserRouter } from "react-router-dom";
import { Content, Footer, Header } from "@components/index";
const App: React.FC = () => {
  return (
    <BrowserRouter>
      <AntApp>
        <Header />
        <Content />
        <Footer />
      </AntApp>
    </BrowserRouter>
  );
};
export default App;
