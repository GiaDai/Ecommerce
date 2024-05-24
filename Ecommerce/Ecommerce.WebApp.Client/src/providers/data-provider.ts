import { DataProvider as BaseDataProvider, HttpError } from "@refinedev/core";
import { ResponseManyRoot, ResponseRoot } from "./types";

const API_URL = "/api";

const processErrorResponse = async (response: Response): Promise<never> => {
  const errorResponse = (await response.json()) as ResponseRoot;
  const error: HttpError = {
    message: errorResponse.Message,
    statusCode: errorResponse.Code,
  };
  return Promise.reject(error);
};

function handleErrorResponse(errorResponse: ResponseRoot): Promise<never> {
  let errorMessage = errorResponse.Message;
  if (
    Array.isArray(errorResponse.Errors) &&
    errorResponse.Errors.every((item) => typeof item === "string")
  ) {
    errorMessage = errorResponse.Errors.join(" ");
  }
  const error: HttpError = {
    message: errorMessage,
    statusCode: errorResponse.Code,
  };
  return Promise.reject(error);
}

const fetcher = async (url: string, options?: RequestInit) => {
  return fetch(url, {
    ...options,
    headers: {
      ...options?.headers,
      Authorization: `Bearer ${localStorage.getItem("access_token") ?? ""}`,
    },
  });
};

export interface DataProvider extends BaseDataProvider {
  getApiUrl(): string;
  deleteFile(publicId: string): void;
  getUserLdap(username: string): any;
}

export const dataProvider: DataProvider = {
  getList: async ({ resource, pagination, filters, sorters }) => {
    const params = new URLSearchParams();
    if (pagination) {
      params.append(
        "_start",
        (
          ((pagination?.current || 1) - 1) *
          (pagination?.pageSize ?? 0)
        ).toString()
      );
      params.append(
        "_end",
        ((pagination?.current || 1) * (pagination?.pageSize ?? 0)).toString()
      );
    }

    if (sorters && sorters.length > 0) {
      params.append("_sort", sorters.map((sorter) => sorter.field).join(","));
      params.append("_order", sorters.map((sorter) => sorter.order).join(","));
    }

    if (filters && filters.length > 0) {
      filters.forEach((filter) => {
        if ("field" in filter && filter.operator === "eq") {
          // Our fake API supports "eq" operator by simply appending the field name and value to the query string.
          // params.append(filter.field, filter.value);
          params.append("_filter", `${filter.field}:${filter.value}`);
        }
        if ("field" in filter && filter.operator === "in") {
          const start = filter.value[0];
          const end = filter.value[1];
          if (start && end) {
            const startDate = `${start.$y}-${start.$M + 1}-${start.$D}`;
            const endDate = `${end.$y}-${end.$M + 1}-${end.$D}`;
            params.append("_filter", `${filter.field}:${startDate}#${endDate}`);
          }
        }
      });
    }

    const response = await fetcher(
      `${API_URL}/${resource}?${params.toString()}`
    );

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      return handleErrorResponse(errorResponse);
    }

    const data = (await response.json()) as ResponseRoot;
    if (!data.Succeeded) {
      const error: HttpError = {
        message: data.Message,
        statusCode: data.Code,
      };
      return Promise.reject(error);
    }

    const total = data.Data._total;
    return {
      data: data.Data._data,
      total,
    };
  },
  getOne: async ({ resource, id }) => {
    const response = await fetcher(`${API_URL}/${resource}/show/${id}`);

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      return handleErrorResponse(errorResponse);
    }
    const data = (await response.json()) as ResponseRoot;
    return { data: data.Data as any };
  },
  getMany: async ({ resource, ids }) => {
    const params = new URLSearchParams();
    if (ids && ids.length > 0) {
      ids.forEach((id) => params.append("id", String(id)));
      const response = await fetcher(
        `${API_URL}/${resource}?${params.toString()}`
      );

      if (response.status === 401) {
        // await authProvider.refresh();
        return processErrorResponse(response);
      }

      if (!response.ok) {
        return processErrorResponse(response);
      } else {
        const data = (await response.json()) as ResponseManyRoot;
        return { data: data.Data as any[] };
      }
    }
    return { data: [] };
  },
  create: async ({ resource, variables }) => {
    const response = await fetcher(`${API_URL}/${resource}`, {
      method: "POST",
      body: JSON.stringify(variables),
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      return handleErrorResponse(errorResponse);
    }
    const data = (await response.json()) as ResponseRoot;
    return { data: data.Data as any };
  },
  createMany: async ({ resource, variables }) => {
    variables = variables as any[];

    const response = await fetcher(`${API_URL}/${resource}/range`, {
      method: "POST",
      body: JSON.stringify(variables),
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseManyRoot;
      const error: HttpError = {
        message: errorResponse.Message,
        statusCode: errorResponse.Code,
      };
      return Promise.reject(error);
    }
    const data = (await response.json()) as ResponseManyRoot;
    return { data: data.Data as any };
  },
  update: async ({ resource, id, variables }) => {
    const response = await fetcher(`${API_URL}/${resource}/${id}`, {
      method: "PUT",
      body: JSON.stringify(variables),
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      const error: HttpError = {
        message: errorResponse.Message,
        statusCode: errorResponse.Code,
      };
      return Promise.reject(error);
    }
    const data = (await response.json()) as ResponseRoot;
    return { data: data.Data as any };
  },
  deleteOne: async ({ resource, id }) => {
    const response = await fetcher(`${API_URL}/${resource}/${id}`, {
      method: "DELETE",
    });

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      const error: HttpError = {
        message: errorResponse.Message,
        statusCode: errorResponse.Code,
      };
      return Promise.reject(error);
    }
    const data = (await response.json()) as ResponseRoot;
    return { data: data.Data as any };
  },
  deleteMany: async ({ resource, ids }) => {
    const response = await fetcher(`${API_URL}/${resource}/range`, {
      method: "DELETE",
      body: JSON.stringify(ids),
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      const error: HttpError = {
        message: errorResponse.Message,
        statusCode: errorResponse.Code,
      };
      return Promise.reject(error);
    }
    const data = (await response.json()) as ResponseRoot;
    return { data: data.Data as any };
  },
  getApiUrl: function (): string {
    throw new Error("Function not implemented.");
  },
  deleteFile: async (publicId): Promise<boolean> => {
    const response = await fetcher(`${API_URL}/file?publicId=${publicId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    return response.ok;
  },
  getUserLdap: async (username: string) => {
    const response = await fetcher(
      `${API_URL}/users/user-ldap?username=${username}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (response.status === 401) {
      // await authProvider.refresh();
      return processErrorResponse(response);
    }

    if (!response.ok) {
      const errorResponse = (await response.json()) as ResponseRoot;
      const error: HttpError = {
        message: errorResponse.Message,
        statusCode: errorResponse.Code,
      };
      return Promise.reject(error);
    }
    const data = (await response.json()) as ResponseRoot;
    return { data: data.Data as any };
  },
};
