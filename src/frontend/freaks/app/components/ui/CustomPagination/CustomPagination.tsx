"use client";
import { Pagination, PaginationItem } from "@mui/material";
import Link from "next/link";
import styles from "./_styles.module.scss";

type Props = {
  url: string;
  page: number;
  searchParamsTag: string | undefined;
  itemInPage: number;
  count: number;
};

const CustomPagination = ({
  url,
  page,
  searchParamsTag,
  itemInPage,
  count,
}: Props) => {
  const pageCount = Math.ceil(count / itemInPage);

  return (
    <Pagination
      classes={{
        root: styles.rootPagination,
        ul: styles.listPagination,
      }}
      shape="rounded"
      page={page}
      count={pageCount}
      size="small"
      renderItem={(item) => (
        <PaginationItem
          classes={{
            selected: styles.active,
            root: styles.itemPagination,
          }}
          component={Link}
          href={`${url}${
            item.page === 1
              ? `${searchParamsTag ? `?tag=${searchParamsTag}` : ""}`
              : `?page=${item.page}${
                  searchParamsTag ? `&tag=${searchParamsTag}` : ""
                }`
          }#bannerSection`}
          {...item}
        />
      )}
    />
  );
};

export default CustomPagination;
