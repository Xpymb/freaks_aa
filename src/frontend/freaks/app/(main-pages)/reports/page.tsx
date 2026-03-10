"use client";

import { useState } from "react";
import ReportList from "./components/ReportList/ReportList";
import ReportStepper from "./components/ReportStepper/ReportStepper";

type Mode = "list" | "stepper";

const Page = () => {
  const [mode, setMode] = useState<Mode>("list");
  const [editingSalaryId, setEditingSalaryId] = useState<number | undefined>();

  const handleCreateNew = () => {
    setEditingSalaryId(undefined);
    setMode("stepper");
  };

  const handleEdit = (salaryId: number) => {
    setEditingSalaryId(salaryId);
    setMode("stepper");
  };

  const handleClose = () => {
    setMode("list");
    setEditingSalaryId(undefined);
  };

  return (
    <>
      {mode === "list" ? (
        <ReportList onCreateNew={handleCreateNew} onEdit={handleEdit} />
      ) : (
        <ReportStepper onClose={handleClose} salaryId={editingSalaryId} />
      )}
    </>
  );
};

export default Page;
