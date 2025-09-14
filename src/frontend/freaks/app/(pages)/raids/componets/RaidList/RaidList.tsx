"use client";

import React, { useState } from "react";
import styles from "./_styles.module.scss";
import { useGetRaids } from "@/domains/raids/hooks/useGetRaids";
import RaidsFilters from "../RaidsFilters/RaidsFilters";

import RaidCard from "../RaidCard/RaidCard";
import { Divider } from "@mui/material";
import Link from "next/link";
import { RaidListQuery } from "@/domains/raids/raids.service";
import CreateRaidForm from "../CreateRaidForm/CreateRaidForm";
import { RaidListItem } from "@/domains/raids/types";
import { PaginatedList } from "@/types/paginated.types";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";

type Props = {
  prefetchRaids: PaginatedList<RaidListItem> | null;
};

const RaidList = ({ prefetchRaids }: Props) => {
  const [filters, setFilters] = useState<Partial<RaidListQuery>>({
    SortBy: 2,
    SortMode: 2,
  });

  const { raids } = useGetRaids(prefetchRaids, filters);

  if (!prefetchRaids) {
    return (
      <ErrorLoadData message="Произошла ошибка при загрузке списка рейдов..." />
    );
  }

  return (
    <div className={styles.wrapper}>
      <RaidsFilters initial={filters} onApply={setFilters} />

      <CreateRaidForm />

      <div className={styles.raids}>
        {raids.map((raid) => (
          <React.Fragment key={raid.id}>
            <Link href={`/raids/${raid.id}`} className={styles.link}>
              <RaidCard raid={raid} />
            </Link>
            <Divider />
          </React.Fragment>
        ))}
      </div>
    </div>
  );
};

export default RaidList;
