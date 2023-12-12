<script setup>
import { ref, computed } from 'vue';
import { vOnClickOutside } from '@vueuse/components';

import SlChip from '@/components/common/SlChip.vue';
import SlIconArrow from '@/components/common/icons/SlIconArrow.vue';
import SlSearchField from '@/components/common/SlSearchField.vue';

const props = defineProps({
  label: {
    type: String,
    required: true,
  },
  options: {
    type: Array,
    required: true,
  },
  tabindex: {
    type: Number,
    default: 0,
  },
  showSearch: {
    type: Boolean,
    default: false,
  },
  value: Array,
  error: String,
});

const emit = defineEmits({ selectValues: null, updateOptions: null });

const search = ref('');
const showPanel = ref(false);

const selectedOptions = computed(() => props.value ?? []);
const hasError = computed(() => !!props.error);
const isEmptyList = computed(() => props.options.length < 1);
const isEmptyCurrentList = computed(() => selectedOptions.value.length < 1);

const togglePanel = () => {
  showPanel.value = !showPanel.value;
};

const closePanel = () => {
  handleSearch('');
  showPanel.value = false;
};

const isSelectedOption = (option) =>
  selectedOptions.value.some((selectedOption) => selectedOption.id === option?.id);

const handleSelectValue = (option) => {
  if (isSelectedOption(option)) return;

  emit('selectValues', option);
};

const handleSearch = (value) => {
  search.value = value;
  emit('updateOptions', value);
};
</script>

<template>
  <div>
    <label class="text-xs mb-2">{{ label }}</label>
    <div
      class="sl-multiselect-container sl-scroll"
      :tabindex="tabindex"
      v-on-click-outside="closePanel"
    >
      <div
        class="sl-multiselect"
        :class="{
          'sl-error': hasError,
          'sl-multiselect-focus': showPanel,
          'sl-error-focus': hasError && showPanel,
          selected: !isEmptyCurrentList,
        }"
        @click="togglePanel"
      >
        <div v-if="!isEmptyCurrentList" class="flex flex-wrap gap-2 max-h-28 overflow-y-auto">
          <SlChip v-for="option in selectedOptions" :key="option.id">
            <slot name="multiSelectItem" :item="option"></slot>
          </SlChip>
        </div>
        <span class="text-gray-500" v-else> Select items </span>
        <SlIconArrow
          class="h-5 w-5 mt-1 stroke-2 rotate-90 shrink-0"
          :class="{ 'text-gray-500': isEmptyCurrentList }"
        />
      </div>

      <ul v-show="showPanel" class="sl-multiselect-menu">
        <template v-if="showSearch">
          <SlSearchField
            :value="search"
            placeholder="Search"
            class="w-full px-1.5 py-1"
            @changeValue="handleSearch"
          />
        </template>
        <li
          v-for="option in options"
          :key="option"
          :class="{
            selected: isSelectedOption(option),
          }"
          class="sl-multiselect-menu-item"
          @click="() => handleSelectValue(option)"
        >
          <slot name="panelItem" :item="option"></slot>
        </li>
        <li v-if="isEmptyList" class="sl-multiselect-menu-item" @click="closePanel">
          This list is empty
        </li>
      </ul>
    </div>
    <div v-if="hasError" class="text-xs text-red-500 mt-1">
      {{ error }}
    </div>
  </div>
</template>

<style scoped>
.sl-multiselect {
  @apply inline-flex justify-between items-center border rounded-lg w-full gap-3 px-3 py-3.5 border-primary-500 hover:cursor-pointer;

  &.selected {
    @apply p-2;
  }
}

.sl-multiselect-focus {
  @apply ring-1 border-primary-700 ring-primary-700;
}

.sl-multiselect-menu {
  @apply absolute top-full z-10 w-full max-h-64 mt-2 overflow-auto rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none;
}

.sl-multiselect-menu-item {
  @apply block text-gray-700 rounded hover:text-gray-800 hover:bg-gray-10% mx-1.5 my-1 px-3.5 py-2 hover:cursor-pointer;

  &.selected {
    @apply border-l-4 border-primary-500 bg-gray-20% hover:cursor-auto;
  }
}

.sl-error {
  @apply border-red-500;
}

.sl-error-focus {
  @apply ring-1 text-black border-red-500 ring-red-500;
}

.sl-multiselect-container {
  @apply relative text-left;

  &:focus {
    @apply rounded-lg;
  }
}
</style>
