<script setup>
import { ref, computed, onMounted } from 'vue';

import SlSearchField from '@/components/common/SlSearchField.vue';
import SlButton from '@/components/common/SlButton.vue';
import SlIconChevronDouble from '@/components/common/icons/SlIconChevronDouble.vue';
import SlIconCaret from '@/components/common/icons/SlIconCaret.vue';
import SlIconPencil from '@/components/common/icons/SlIconPencil.vue';
import SlIconTrash from '@/components/common/icons/SlIconTrash.vue';

const SORT_ORDER = ['ASC', 'DESC'];

const props = defineProps({
  headers: {
    type: Array,
    required: true,
  },
  items: {
    type: [Array, null],
    required: true,
  },
  defaultTableOptions: {
    type: Object,
    default: () => {
      return {
        search: '',
        sort: '',
        page: 1,
        pageCount: 50,
      };
    },
  },
  loading: Boolean,
});

const emit = defineEmits({ tableChange: null, addItem: null, editItem: null, deleteItem: null });

const tableOptions = ref({});

const emptyState = computed(() => props.items.length < 1);
const canMoveToPreviousPage = computed(() => tableOptions.value.page > 1);
const canMoveToNextPage = computed(() => tableOptions.value.page < tableOptions.value?.pageCount);
const isSortOrderAsc = computed(() => tableOptions.value?.sort?.includes(SORT_ORDER[0]));

const updateTable = () => {
  emit('tableChange', tableOptions.value);
};

const updateTableOptions = (value, shouldUpdateTable = true) => {
  const newTableOptions = { ...tableOptions.value, ...value };
  tableOptions.value = newTableOptions;

  if (shouldUpdateTable) {
    updateTable();
  }
};

const checkSortStatus = (sortName) => {
  if (!tableOptions.value?.sort) return false;

  const name = tableOptions.value?.sort.replace(/:ASC|:DESC/i, '');
  return name === sortName;
};

const setSortOrder = (value) => {
  const sortOrder =
    tableOptions.value?.sort.includes(value) && tableOptions.value?.sort.includes(SORT_ORDER[0])
      ? SORT_ORDER[1]
      : SORT_ORDER[0];

  updateTableOptions({ sort: `${value}:${sortOrder}` });
};

const previousPage = () =>
  updateTableOptions({
    page: --tableOptions.value.page,
  });
const nextPage = () =>
  updateTableOptions({
    page: ++tableOptions.value.page,
  });

defineExpose({
  updateTable,
  updateTableOptions,
});

onMounted(() => {
  updateTableOptions(props.defaultTableOptions);
});
</script>

<template>
  <div class="sl-scroll relative flex flex-col justify-start h-full w-full p-6">
    <div class="flex flex-row justify-between pb-6">
      <div class="w-80">
        <SlSearchField
          placeholder="Search for items"
          @changeValue="(value) => updateTableOptions({ search: value })"
        />
      </div>
      <div v-if="$slots.tableHeader" class="h-full">
        <slot name="tableHeader"></slot>
      </div>
    </div>

    <div class="overflow-auto">
      <table class="relative border rounded-lg shadow text-sm text-left text-gray-500 w-full">
        <thead class="sticky -top-0.5 z-10 shadow text-xs text-gray-700 uppercase bg-gray-100">
          <tr>
            <th
              v-for="header in headers"
              :key="header"
              scope="col"
              class="px-4 py-3.5"
              :class="header.styleClass"
            >
              <div
                v-if="!header?.hideName"
                class="flex flex-row items-center"
                :class="[header.elementClass, { 'hover:cursor-pointer': header?.sortName }]"
                @click="() => (header?.sortName ? setSortOrder(header?.sortName) : null)"
              >
                <span>
                  {{ header.name }}
                </span>
                <div v-if="checkSortStatus(header.sortName)" class="w-2 h-2 ml-3 shrink-0">
                  <SlIconCaret
                    class="w-full h-full mb-0.5"
                    :class="{ 'rotate-180': isSortOrderAsc }"
                  />
                </div>
              </div>
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-if="!emptyState">
            <tr v-for="item in items" :key="item" class="bg-white border-b">
              <slot name="body" :item="item"></slot>
              <td class="w-32 p-4 whitespace-nowrap">
                <div class="flex flex-row justify-center gap-3">
                  <slot name="actions" :item="item"></slot>
                  <SlButton styleClass="btn-link p-1.5" @handleClick="() => $emit('editItem', item)"
                    ><template #icon> <SlIconPencil class="h-5 w-5 stroke-1.5" /> </template
                  ></SlButton>
                  <SlButton
                    styleClass="btn-link p-1.5"
                    @handleClick="() => $emit('deleteItem', item?.id)"
                    ><template #icon>
                      <SlIconTrash class="text-red-500 h-5 w-5 stroke-1.5" /> </template
                  ></SlButton>
                </div>
              </td>
            </tr>
          </template>
          <tr v-else>
            <td class="px-6 py-4 whitespace-nowrap">No Data</td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="tableOptions?.pageCount > 1" class="flex items-center justify-center gap-6 pt-8">
      <div class="w-5 h-5">
        <SlIconChevronDouble
          v-show="canMoveToPreviousPage"
          class="w-full h-full"
          @click="previousPage"
        />
      </div>
      <div>
        <span class="text-sm font-normal text-gray-500"
          >Page <span class="font-semibold text-gray-900">{{ tableOptions?.page }}</span> of
          <span class="font-semibold text-gray-900">{{ tableOptions?.pageCount }}</span></span
        >
      </div>
      <div class="w-5 h-5">
        <SlIconChevronDouble
          v-show="canMoveToNextPage"
          class="w-full h-full rotate-180"
          @click="nextPage"
        />
      </div>
    </div>
  </div>
</template>
