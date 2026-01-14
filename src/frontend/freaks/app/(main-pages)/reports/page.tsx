"use client";

import { useState } from "react";
import { AnimatePresence, motion } from "framer-motion";
import ReportList from "./components/ReportList/ReportList";
import ReportStepper from "./components/ReportStepper/ReportStepper";

type Mode = "list" | "stepper";

const Page = () => {
  const [mode, setMode] = useState<Mode>("list");
  const [editingReportId, setEditingReportId] = useState<number | undefined>();

  const handleCreateNew = () => {
    setEditingReportId(undefined);
    setMode("stepper");
  };

  const handleEdit = (reportId: number) => {
    setEditingReportId(reportId);
    setMode("stepper");
  };

  const handleClose = () => {
    setMode("list");
    setEditingReportId(undefined);
  };

  const pageVariants = {
    initial: { opacity: 0, x: -20 },
    animate: { opacity: 1, x: 0 },
    exit: { opacity: 0, x: 20 },
  };

  return (
    <AnimatePresence mode="wait">
      {mode === "list" ? (
        <motion.div
          key="list"
          variants={pageVariants}
          initial="initial"
          animate="animate"
          exit="exit"
          transition={{ duration: 0.3 }}
        >
          <ReportList onCreateNew={handleCreateNew} onEdit={handleEdit} />
        </motion.div>
      ) : (
        <motion.div
          key="stepper"
          variants={pageVariants}
          initial="initial"
          animate="animate"
          exit="exit"
          transition={{ duration: 0.3 }}
        >
          <ReportStepper onClose={handleClose} reportId={editingReportId} />
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default Page;