import React from "react";
import { useLogin } from "@refinedev/core";

export const Login = () => {
  const { mutate, isLoading } = useLogin();

  const onSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const form = event.target as HTMLFormElement; // Cast event.target to HTMLFormElement
    const data = Object.fromEntries(new FormData(form).entries()); // Use the form variable instead of event.target
    mutate(data);
  };

  return (
    <div>
      <h1>Login</h1>
      <form onSubmit={onSubmit}>
        <label htmlFor="email">Email</label>
        <input
          type="email"
          id="email"
          name="email"
          // We're providing default values for demo purposes.
          defaultValue="basicuser@gmail.com"
        />

        <label htmlFor="password">Password</label>
        <input
          type="password"
          id="password"
          name="password"
          // We're providing default values for demo purposes.
          defaultValue="123Pa$$word!"
        />

        {isLoading && <span>loading...</span>}
        <button type="submit" disabled={isLoading}>
          Submit
        </button>
      </form>
    </div>
  );
};
