<script setup>
import { ref, computed } from 'vue';
import { vOnClickOutside } from '@vueuse/components';

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
  value: Object,
  showSearch: {
    type: Boolean,
    default: false,
  },
  error: String,
});

const emit = defineEmits({ selectValue: null, updateOptions: null });

const search = ref('');
const showPanel = ref(false);

const selectedOption = computed(() => props.value ?? {});
const hasError = computed(() => !!props.error);
const emptyState = computed(() => props.options.length < 1);

const togglePanel = () => {
  showPanel.value = !showPanel.value;
};

const closePanel = () => {
  showPanel.value = false;
};

const isSelectedOption = (option) => selectedOption.value.id === option.id;

const handleSelectValue = (option) => {
  emit('selectValue', option);
  handleSearch('');
  closePanel();
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
      class="sl-dropdown-container sl-scroll"
      :tabindex="tabindex"
      v-on-click-outside="closePanel"
    >
      <div
        class="sl-dropdown"
        :class="{
          'sl-error': hasError,
          'sl-dropdown-focus': showPanel,
          'sl-error-focus': hasError && showPanel,
          'text-gray-500': !selectedOption?.name,
        }"
        @click="togglePanel"
      >
        {{ selectedOption?.name || `Select item` }}
        <SlIconArrow class="h-5 w-5 mt-1 stroke-2 rotate-90" />
      </div>

      <ul v-show="showPanel" class="sl-dropdown-menu">
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
          class="sl-dropdown-menu-item"
          @click="() => handleSelectValue(option)"
        >
          {{ option.name }}
        </li>
        <li v-if="emptyState" class="sl-dropdown-menu-item" @click="closePanel">
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
.sl-dropdown {
  @apply inline-flex justify-between items-center border rounded-lg w-full px-3 py-3.5 border-primary-500 hover:cursor-pointer;
}

.sl-dropdown-focus {
  @apply ring-1 border-primary-700 ring-primary-700;
}

.sl-dropdown-menu {
  @apply absolute top-full z-10 w-full max-h-64 mt-2 overflow-auto rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none;
}

.sl-dropdown-menu-item {
  @apply block text-gray-700 rounded hover:text-gray-800 hover:bg-gray-10% mx-1.5 my-1 px-3.5 py-2 hover:cursor-pointer;

  &.selected {
    @apply border-l-4 border-primary-500  bg-gray-20%;
  }
}

.sl-error {
  @apply border-red-500;
}

.sl-error-focus {
  @apply ring-1 text-black border-red-500 ring-red-500;
}

.sl-dropdown-container {
  @apply relative text-left;

  &:focus {
    @apply rounded-lg;
  }
}
</style>
