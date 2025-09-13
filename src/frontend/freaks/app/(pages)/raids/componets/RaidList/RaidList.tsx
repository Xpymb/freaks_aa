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

const RaidList = () => {
  const [filters, setFilters] = useState<Partial<RaidListQuery>>({
    SortBy: 2, // сортировка по дате создания (предполагаем, что 1 = createdDt)
    SortMode: 2, // убывание (предполагаем, что 1 = DESC)
  });

  const { raids } = useGetRaids(filters);

  // как было: ранние return (знай, что они размонтируют фильтры)
  // if (isLoading || !raids) return <DefaultLoader />;
  // if (errorState.isError) return <h1>{errorState.message}</h1>;

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
