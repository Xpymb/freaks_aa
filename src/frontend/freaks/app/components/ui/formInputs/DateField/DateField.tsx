"use client";

import * as React from "react";
import { Button, IconButton } from "@mui/material";
import { DayPicker, DateRange } from "react-day-picker";
import "react-day-picker/dist/style.css";
import { ru } from "date-fns/locale";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";

import {
  Control,
  FieldValues,
  Path,
  useController,
} from "react-hook-form";

import CustomModal from "@/components/ui/CustomModal/CustomModal";
import CheckIcon from "@mui/icons-material/Check";
import CloseIcon from "@mui/icons-material/Close";
import RestartAltIcon from "@mui/icons-material/RestartAlt";

import { useDisclosure } from "../../useDisclosure";
import styles from "./_styles.module.scss";
import { CustomTypography } from "../../CustomTypography";

type Mode = "single" | "range" | "multiple";

type Props<T extends FieldValues> = {
  control: Control<T>;
  nameFrom: Path<T>;
  nameTo: Path<T>;
  label?: string;
  initialMonths?: number;
  monthsOptions?: number[];
  disabled?: boolean;
};

const toDate = (iso?: string) => (iso ? new Date(iso) : undefined);
const toISO = (d?: Date) => (d ? d.toISOString() : undefined);

function getModeProps(
  mode: Mode,
  draftSingle: Date | undefined,
  setDraftSingle: (d?: Date) => void,
  draftRange: DateRange | undefined,
  setDraftRange: (r?: DateRange) => void,
  draftMultiple: Date[],
  setDraftMultiple: (d: Date[]) => void
) {
  switch (mode) {
    case "single":
      return {
        mode: "single" as const,
        selected: draftSingle,
        onSelect: (val: Date | undefined) => setDraftSingle(val ?? undefined),
      };
    case "range":
      return {
        mode: "range" as const,
        selected: draftRange,
        onSelect: (val: DateRange | undefined) =>
          setDraftRange(val ?? undefined),
      };
    case "multiple":
      return {
        mode: "multiple" as const,
        selected: draftMultiple,
        onSelect: (val: Date[] | undefined) => setDraftMultiple(val ?? []),
      };
  }
}

function SmartDayPicker({
  mode,
  months,
  draftSingle,
  setDraftSingle,
  draftRange,
  setDraftRange,
  draftMultiple,
  setDraftMultiple,
}: {
  mode: Mode;
  months: number;
  draftSingle: Date | undefined;
  setDraftSingle: (d?: Date) => void;
  draftRange: DateRange | undefined;
  setDraftRange: (r?: DateRange) => void;
  draftMultiple: Date[];
  setDraftMultiple: (d: Date[]) => void;
}) {
  return (
    <DayPicker
      numberOfMonths={months}
      weekStartsOn={1}
      locale={ru}
      classNames={{
        range_start: styles.rangBackgroundStart,
        range_middle: styles.rangBackgroundMiddle,
        range_end: styles.rangBackgroundEnd,
      }}
      {...getModeProps(
        mode,
        draftSingle,
        setDraftSingle,
        draftRange,
        setDraftRange,
        draftMultiple,
        setDraftMultiple
      )}
    />
  );
}

export default function DateOrRangeField<T extends FieldValues>({
  control,
  nameFrom,
  nameTo,
  label = "Дата",
  initialMonths = 1,
  // monthsOptions = [1, 2, 3, 4, 5, 6],
  disabled,
}: Props<T>) {
  const fromCtl = useController({ control, name: nameFrom });
  const toCtl = useController({ control, name: nameTo });

  const currentFrom = toDate(fromCtl.field.value as string | undefined);
  const currentTo = toDate(toCtl.field.value as string | undefined);

  const { open, onOpen, onClose } = useDisclosure(false);

  // 👉 фиксируем сразу "range"
  const [mode] = React.useState<Mode>("range");
  const [months] = React.useState(initialMonths);

  const [draftSingle, setDraftSingle] = React.useState<Date | undefined>();
  const [draftRange, setDraftRange] = React.useState<DateRange | undefined>();
  const [draftMultiple, setDraftMultiple] = React.useState<Date[]>([]);

  const handleOpen = () => {
    if (currentFrom && currentTo) {
      setDraftRange({ from: currentFrom, to: currentTo });
    } else {
      setDraftRange(undefined);
    }
    setDraftSingle(undefined);
    setDraftMultiple([]);
    onOpen();
  };

  const apply = () => {
    fromCtl.field.onChange(toISO(draftRange?.from));
    toCtl.field.onChange(toISO(draftRange?.to));
    onClose();
  };

  // const monthsField: ControllerRenderProps = React.useMemo(
  //   () => ({
  //     name: "months",
  //     value: months,
  //     onChange: (e) => setMonths(Number((e.target as HTMLInputElement).value)),
  //     onBlur: () => {},
  //     ref: () => {},
  //   }),
  //   [months]
  // );

  // const monthOptions = React.useMemo(
  //   () => monthsOptions.map((n) => ({ value: n, label: String(n) })),
  //   [monthsOptions]
  // );

  return (
    <div className={styles.wrapper}>
      <Button
        onClick={!disabled ? handleOpen : undefined}
        className={styles.modalOpen}
      >
        <CustomTypography variant="caption">{label}</CustomTypography>
        <CalendarMonthIcon />
      </Button>

      <CustomModal
        className={styles.modal}
        open={open}
        onClose={onClose}
        title="Выбор периода"
        minWidth={"0"}
      >
        {/* Панель управления */}
        <div>
          {/* <div style={{ display: "flex", gap: 8 }}>
            <Button
              variant={mode === "single" ? "contained" : "outlined"}
              size="small"
              onClick={() => setMode("single")}
            >
              Дата
            </Button>
            <Button
              variant={mode === "range" ? "contained" : "outlined"}
              size="small"
              onClick={() => setMode("range")}
            >
              Период
            </Button>
            <Button
              variant={mode === "multiple" ? "contained" : "outlined"}
              size="small"
              onClick={() => setMode("multiple")}
            >
              Несколько
            </Button>
          </div> */}

          {/* <div>
            <CustomSelect
              name="months"
              label="Месяцев"
              options={monthOptions}
              field={monthsField}
              size="small"
              MenuProps={{ disablePortal: true }}
            />
          </div> */}
        </div>

        {/* Календарь */}
        <div>
          <SmartDayPicker
            mode={mode}
            months={months}
            draftSingle={draftSingle}
            setDraftSingle={setDraftSingle}
            draftRange={draftRange}
            setDraftRange={setDraftRange}
            draftMultiple={draftMultiple}
            setDraftMultiple={setDraftMultiple}
          />
        </div>

        <div className={styles.btnWrapp}>
          <IconButton onClick={apply}>
            <CheckIcon />
          </IconButton>
          <IconButton onClick={onClose}>
            <CloseIcon />
          </IconButton>
          <IconButton>
            <RestartAltIcon />
          </IconButton>
        </div>
      </CustomModal>
    </div>
  );
}
